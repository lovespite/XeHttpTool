using System.Text.Json.Serialization;

namespace XeHttpTool.Model;

internal class XeRequestHeaderCollection : XeHeaderCollection
{
    public override XeHeaderCollection Copy()
    {
        var copy = new XeRequestHeaderCollection();
        foreach (var header in this) copy.Add(header.Copy());
        return copy;
    }

    #region Quick Accessors for Common Request Headers

    [JsonIgnore]
    public string? UserAgent
    {
        get => GetHeader(XeHeader.CommonHeaders.UserAgent);
        set => SetHeader(XeHeader.CommonHeaders.UserAgent, value);
    }

    [JsonIgnore]
    public string? Accept
    {
        get => GetHeader(XeHeader.CommonHeaders.Accept);
        set => SetHeader(XeHeader.CommonHeaders.Accept, value);
    }

    [JsonIgnore]
    public string? Authorization
    {
        get => GetHeader(XeHeader.CommonHeaders.Authorization);
        set => SetHeader(XeHeader.CommonHeaders.Authorization, value);
    }

    [JsonIgnore]
    public long? ContentLength
    {
        get
        {
            var value = GetHeader(XeHeader.CommonHeaders.ContentLength);
            if (long.TryParse(value, out var result))
                return result;
            return null;
        }
        set
        {
            if (value.HasValue)
                SetHeader(XeHeader.CommonHeaders.ContentLength, value.Value.ToString());
            else
                SetHeader(XeHeader.CommonHeaders.ContentLength, null);
        }
    }

    [JsonIgnore]
    public string? ContentType
    {
        get => GetHeader(XeHeader.CommonHeaders.ContentType);
        set => SetHeader(XeHeader.CommonHeaders.ContentType, value);
    }

    [JsonIgnore]
    public string? AcceptEncoding
    {
        get => GetHeader(XeHeader.CommonHeaders.AcceptEncoding);
        set => SetHeader(XeHeader.CommonHeaders.AcceptEncoding, value);
    }

    [JsonIgnore]
    public string? Cookie
    {
        get => GetHeader(XeHeader.CommonHeaders.Cookie);
        set => SetHeader(XeHeader.CommonHeaders.Cookie, value);
    }

    #endregion
}
