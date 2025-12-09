namespace XeHttpTool.Model;

internal static class XeRequestHeaderCollectionExtensions
{
    public static void CopyToHttpRequestMessage(this XeHeaderCollection headers, HttpRequestMessage message)
    {
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(message);

        foreach (var header in headers)
        {
            if (!message.Headers.TryAddWithoutValidation(header.Name, header.Value))
            {
                message.Content?.Headers.TryAddWithoutValidation(header.Name, header.Value);
            }
        }
    }
}