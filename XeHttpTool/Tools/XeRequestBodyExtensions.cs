using System.Net;
using XeHttpTool.Model;
using Microsoft.AspNetCore.StaticFiles;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XeHttpTool.Tools;

internal static class XeRequestBodyExtensions
{
    private static readonly FileExtensionContentTypeProvider s_ContentTypeProvider = new();

    public static string GetContentTypeName(this FileInfo file)
    {
        if (!s_ContentTypeProvider.TryGetContentType(file.Name, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    public static HttpContent? GetHttpContent(this XeRequestBody body)
    {
        return body.BodyType switch
        {
            XeRequestBodyRawType.Raw => ParseAsByteArrayContent(body),
            XeRequestBodyRawType.FormUrlEncoded => ParseAsFormUrlEncodedContent(body),
            XeRequestBodyRawType.MultipartFormData => ParseAsMfdContent(body),
            _ => null,
        };
    }

    public static async Task<string> GetTextualRepresentation(this HttpContent content)
    {
        using var ms = new MemoryStream();
        await content.CopyToAsync(ms);
        ms.Position = 0;
        using var reader = new StreamReader(ms);
        return await reader.ReadToEndAsync();
    }

    private static FormUrlEncodedContent ParseAsFormUrlEncodedContent(XeRequestBody body)
    {
        Dictionary<string, string> dict = ParseAsFormDictionary(body);
        return new(dict);
    }

    private static ByteArrayContent ParseAsByteArrayContent(XeRequestBody body)
    {
        return body.Format switch
        {
            XeRequestBodyRawFormat.TextJson => new StringContent(body.TextualRepresentation ?? string.Empty, System.Text.Encoding.UTF8, "application/json"),
            XeRequestBodyRawFormat.BinaryBase64 => new ByteArrayContent(Convert.FromBase64String(body.TextualRepresentation ?? string.Empty)),
            XeRequestBodyRawFormat.BinaryHex => new ByteArrayContent(Convert.FromHexString(body.TextualRepresentation ?? string.Empty)),
            _ => new StringContent(body.TextualRepresentation ?? string.Empty, System.Text.Encoding.UTF8, "text/plain"),
        };
    }

    private static MultipartFormDataContent ParseAsMfdContent(XeRequestBody body)
    {
        var dict = ParseAsFormDictionary(body);
        var content = new MultipartFormDataContent();
        var fileRegex = XeFormDataRegexes.File();
        foreach (var kv in dict)
        {
            var match = fileRegex.Match(kv.Value);
            if (match.Success)
            {
                var file = new FileInfo(match.Groups[1].Value);
                if (file.Exists)
                {
                    var fileStream = file.OpenRead();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.GetContentTypeName());
                    content.Add(fileContent, kv.Key, file.Name);
                }
                else
                {
                    throw new FileNotFoundException($"字段 '{kv.Key}' 引用的文件不存在", file.FullName);
                }
            }
            else
            {
                content.Add(new StringContent(kv.Value), kv.Key);
            }
        }
        return content;
    }

    private static Dictionary<string, string> ParseAsFormDictionary(XeRequestBody body)
    {
        var dict = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(body.TextualRepresentation))
        {
            var pairs = body.TextualRepresentation.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                var kv = pair.Split('=', 2);
                var key = WebUtility.UrlDecode(kv[0]);
                var value = kv.Length > 1 ? WebUtility.UrlDecode(kv[1]) : string.Empty;
                dict[key] = value;
            }
        }

        return dict;
    }
}

internal partial class XeFormDataRegexes
{
    [GeneratedRegex(@"\$FILE<(.+?)>", RegexOptions.Singleline)]
    public static partial Regex File();
}