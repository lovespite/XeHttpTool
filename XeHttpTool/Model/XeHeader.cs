namespace XeHttpTool.Model;

internal class XeHeader : XeNamedEntity
{
    public string Value { get; set; } = string.Empty;

    public XeHeader() { }

    public XeHeader(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public XeHeader Copy()
    {
        return new XeHeader
        {
            Name = Name,
            Value = Value,
        };
    }

    public override string ToString()
    {
        return Name + ": " + Value;
    }

    internal static class CommonHeaders
    {
        public const string ContentType = "Content-Type";
        public const string ContentEncoding = "Content-Encoding";
        public const string UserAgent = "User-Agent";
        public const string Server = "Server";
        public const string Accept = "Accept";
        public const string Authorization = "Authorization";
        public const string AcceptEncoding = "Accept-Encoding";
        public const string ContentLength = "Content-Length";
        public const string Cookie = "Cookie";
        public const string SetCookie = "Set-Cookie";
    }

    internal static class CommonContentTypes
    {
        public const string ApplicationJson = "application/json";
        public const string ApplicationXml = "application/xml";
        public const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";
        public const string MultipartFormData = "multipart/form-data";
        public const string TextPlain = "text/plain";
        public const string TextHtml = "text/html";
    }

    internal static class CommonContentEncodings
    {
        public const string Gzip = "gzip";
        public const string Deflate = "deflate";
        public const string Brotli = "br";
    }
}
