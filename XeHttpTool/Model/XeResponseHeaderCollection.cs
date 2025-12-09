using System.Text.Json.Serialization;

namespace XeHttpTool.Model;

internal class XeResponseHeaderCollection : XeHeaderCollection
{
    // Override to make read-only
    public override bool IsReadOnly => true;

    public override XeResponseHeaderCollection Copy() => [];

    #region Quick Accessors for Common Response Headers

    [JsonIgnore]
    public string? ContentEncoding
    {
        get => GetHeader(XeHeader.CommonHeaders.ContentEncoding);
    }

    [JsonIgnore]
    public string? ContentType
    {
        get => GetHeader(XeHeader.CommonHeaders.ContentType);
    }

    //[JsonIgnore]
    //public long? ContentLength
    //{
    //    get
    //    {
    //        var value = GetHeader(XeHttpHeader.CommonHeaders.ContentLength);
    //        if (long.TryParse(value, out var result))
    //            return result;
    //        return null;
    //    }
    //}

    //[JsonIgnore]
    //public string? SetCookie
    //{
    //    get => GetHeader(XeHttpHeader.CommonHeaders.SetCookie);
    //}

    //[JsonIgnore]
    //public string? Server
    //{
    //    get => GetHeader(XeHttpHeader.CommonHeaders.Server);
    //}

    #endregion
}
