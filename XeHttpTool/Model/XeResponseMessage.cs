using System.Text.Json.Serialization;
using XeHttpTool.Tools;

namespace XeHttpTool.Model;

internal class XeResponseMessage : IDisposable
{
    private Stream? m_BufferedFileStream;

    public XeResponseMessage() { }

    private XeResponseMessage(Stream bufferedStream, XeResponseHeaderCollection headers)
    {
        m_BufferedFileStream = bufferedStream;
        Headers = headers;
    }

    [JsonIgnore] public string? RequestUrl => Request?.Url;

    public XeRequestMessage? Request { get; set; }
    public XeResponseHeaderCollection Headers { get; set; } = [];
    public string? BufferFilePath { get; set; }
    public long SizeInBytes { get; set; } = 0;
    public int StatusCode { get; set; } = 0;
    public string? StatusPhrase { get; set; }

    public async Task<string> ReadAsStringAsync(System.Text.Encoding encoding)
    {
        if (m_BufferedFileStream is null)
        {
            if (!File.Exists(BufferFilePath)) return string.Empty;
            m_BufferedFileStream = File.OpenRead(BufferFilePath);
        }

        m_BufferedFileStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(m_BufferedFileStream, encoding, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }

    public async Task<string> ReadAsStringAsync()
    {
        var charset = TextEncodingTool.GetCharsetFromContentType(Headers.ContentType);
        var encoding = TextEncodingTool.GetEncodingFromCharsetName(charset);
        return await ReadAsStringAsync(encoding);
    }

    public static async Task<XeResponseMessage> FromStreamAsync(XeRequestMessage request,
                                                                Stream responseStream,
                                                                int responseStatusCode,
                                                                string responseStatusMessage,
                                                                XeResponseHeaderCollection headers,
                                                                bool deleteOnClose = true,
                                                                bool encrypted = false)
    {
        var options = new ResponseStreamOptions
        {
            Encrypted = encrypted,
            DeleteOnClose = deleteOnClose,
        };
        var bufferStream = await CreateFileStreamBuffer(responseStream, options);

        return new XeResponseMessage(bufferStream, headers)
        {
            Request = request,
            SizeInBytes = bufferStream.Length,
            StatusCode = responseStatusCode,
            StatusPhrase = responseStatusMessage,
            BufferFilePath = bufferStream.Name,
        };
    }

    private static async Task<FileStream> CreateFileStreamBuffer(Stream responseStream, ResponseStreamOptions options)
    {
        var tempFilePath = Path.GetTempFileName();

        using (var tempFs = File.Create(tempFilePath))
        {
            await responseStream.CopyToAsync(tempFs);
            await tempFs.FlushAsync();
        }

        return new FileStream(path: tempFilePath,
                              mode: FileMode.Open,
                              access: FileAccess.Read,
                              share: FileShare.Read,
                              bufferSize: 4096,
                              options: options.GetOptions());
    }

    public void Dispose()
    {
        m_BufferedFileStream?.Dispose();
    }

    internal struct ResponseStreamOptions()
    {
        public static readonly ResponseStreamOptions Default = new();

        /// <summary>
        /// Gets or sets a value indicating whether the underlying resource should be deleted when the object is disposed.
        /// </summary>
        /// <remarks>Set this property to <see langword="false"/> to retain the resource after disposal. By
        /// default, the resource is deleted when the object is disposed.</remarks>
        public bool DeleteOnClose { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the content is encrypted. 
        /// </summary>
        /// <remarks> 
        /// This feature is implemented by the operation system and is not related to any specific encryption algorithm.
        /// </remarks>
        public bool Encrypted { get; set; } = false;

        public readonly FileOptions GetOptions()
        {
            var options = FileOptions.None;
            if (DeleteOnClose)
            {
                options |= FileOptions.DeleteOnClose;
            }
            if (Encrypted)
            {
                options |= FileOptions.Encrypted;
            }
            return options;
        }
    }
}
