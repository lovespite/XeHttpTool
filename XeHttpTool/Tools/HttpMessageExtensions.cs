using XeHttpTool.Model;

namespace XeHttpTool.Tools;

internal static class HttpMessageExtensions
{
    public static async Task<XeResponseMessage> ToXeResponseMessage(this HttpResponseMessage message, XeRequestMessage request, bool deleteOnClose = true, bool encrypted = false)
    {
        var headers = new XeResponseHeaderCollection();

        foreach (var header in message.Headers)
        {
            foreach (var value in header.Value)
            {
                headers.Add(new XeHeader(header.Key, value));
            }
        }

        var responseStream = message.Content.ReadAsStream();
        var responseMessage = await XeResponseMessage.FromStreamAsync(request : request,
                                                                  responseStream: responseStream,
                                                                  responseStatusCode: (int)message.StatusCode,
                                                                  responseStatusMessage: message.ReasonPhrase ?? string.Empty,
                                                                  headers: headers,
                                                                  deleteOnClose: deleteOnClose,
                                                                  encrypted: encrypted);

        return responseMessage;
    }
}
