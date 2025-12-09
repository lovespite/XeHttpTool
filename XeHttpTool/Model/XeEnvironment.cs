namespace XeHttpTool.Model;

internal class XeEnvironment
{
    /// <summary>
    /// Gets or sets the address of the proxy server to use for network requests.<br />
    /// Eg.: "http://127.0.0.1:8080"
    /// </summary>
    public string? ProxyAddress { get; set; }

    /// <summary>
    /// Gets or sets the base URI used for requests.
    /// </summary>
    /// <remarks>Set this property to specify the root address for all outgoing requests. If not set, requests
    /// may require an absolute URI.</remarks>
    public string? BaseAddress { get; set; }

    #region Variables

    public Dictionary<string, string> Variables
    {
        get => m_Variables;
        set => m_Variables = new Dictionary<string, string>(value);
    }

    private Dictionary<string, string> m_Variables = [];

    public string? GetVariable(string name)
    {
        if (m_Variables.TryGetValue(name, out var value))
            return value;
        return null;
    }

    public void SetVariable(string name, string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (value is null)
            m_Variables.Remove(name);
        else
            m_Variables[name] = value;
    }

    #endregion

    #region Clone

    public XeEnvironment Copy()
    {
        var clone = new XeEnvironment
        {
            m_Variables = new Dictionary<string, string>(m_Variables),
            ProxyAddress = this.ProxyAddress,
            BaseAddress = this.BaseAddress,
        };
        return clone;
    }

    #endregion

}
