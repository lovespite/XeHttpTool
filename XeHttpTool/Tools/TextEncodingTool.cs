using System.Diagnostics;

namespace XeHttpTool.Tools;

internal class TextEncodingTool
{
    internal static string? GetCharsetFromContentType(string? contentType)
    {
        if (contentType is null) return null;
        var parts = contentType.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();
            if (trimmedPart.StartsWith("charset=", StringComparison.OrdinalIgnoreCase))
            {
                return trimmedPart["charset=".Length..].Trim().ToLowerInvariant();
            }
        }
        return null;
    }

    internal static System.Text.Encoding GetEncodingFromCharsetName(string? charset)
    {
        if (string.IsNullOrWhiteSpace(charset))
        {
            return System.Text.Encoding.UTF8;
        }
        try
        {
            return System.Text.Encoding.GetEncoding(charset);
        }
        catch (ArgumentException)
        {
            Debug.WriteLine($"[TextEncodingTool] Unknown charset '{charset}', defaulting to UTF-8.");
            return System.Text.Encoding.UTF8;
        }
    }
}