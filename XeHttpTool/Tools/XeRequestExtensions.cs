using XeHttpTool.Model;

namespace XeHttpTool.Tools;

internal static class XeRequestExtensions
{
    public static XeRequestMessage ToXeRequestMessage(this XeRequest request)
    {
        return new XeRequestMessage
        {
            Method = request.Method,
            Url = request.Url,
            Headers = (XeRequestHeaderCollection)request.Headers.Copy(),
            Body = request.Body.TextualRepresentation,
            BodyType = request.Body.BodyType,
            Format = request.Body.Format,
        };
    }
}
