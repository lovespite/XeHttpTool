using System.Text.Json.Serialization;

namespace XeHttpTool.Model;

internal class XeRequest() : XeNamedEntity
{

    public string Id { get; set; } = string.Empty;
    public string Method { get; set; } = Methods.Get;
    public string Url { get; set; } = string.Empty;
    public XeScripts Scripts { get; set; } = new();
    public XeRequestHeaderCollection Headers { get; set; } = [];
    public XeRequestBody Body { get; set; } = XeRequestBody.Empty;

    public override string ToString()
    {
        return "XeRequest: " + Name;
    }

    public XeRequest Copy()
    {
        return new XeRequest()
        {
            Name = Name,
            Method = Method,
            Url = Url,
            Scripts = Scripts.Copy(),
            Headers = (XeRequestHeaderCollection)Headers.Copy(),
            Body = Body.Copy(),
        };
    }

    internal static class Methods
    {
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "PUT";
        public const string Delete = "DELETE";
        public const string Patch = "PATCH";
        public const string Head = "HEAD";
        public const string Options = "OPTIONS";
    }
}
