namespace XeHttpTool.Model;

internal class XeScripts
{
    /// <summary>
    /// Gets or sets the script to be executed before sending the request.
    /// </summary>
    public string? PreRequest { get; set; }

    /// <summary>
    /// Gets or sets the test script to be executed after receiving the response.
    /// </summary>
    public string? Test { get; set; }

    public XeScripts Copy()
    {
        return new XeScripts
        {
            PreRequest = PreRequest,
            Test = Test,
        };
    }
}