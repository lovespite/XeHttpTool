namespace XeHttpTool.Model;

internal class XeRequestMessage
{
    public string Method { get; set; } = "GET";
    public string Url { get; set; } = string.Empty;
    public XeRequestHeaderCollection Headers { get; set; } = [];
    public string? Body { get; set; }
    public XeRequestBodyRawType BodyType { get; set; }
    public XeRequestBodyRawFormat Format { get; set; }
}
