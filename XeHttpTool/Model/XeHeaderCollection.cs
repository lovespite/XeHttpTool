namespace XeHttpTool.Model;

internal abstract class XeHeaderCollection : XeNamedCollection<XeHeader>
{
    public new string? this[string name]
    {
        get => GetHeader(name);
        set => SetHeader(name, value);
    }

    /// <summary>
    /// Gets an enumerable collection of unique header names present in the current set of items.
    /// </summary>
    /// <remarks>Header name comparisons are performed using case-insensitive ordinal rules. The returned
    /// collection reflects the distinct header names currently available and does not include duplicates differing only
    /// by case.</remarks>
    public IEnumerable<string> HeaderNames => m_Items.Select(h => h.Name).Distinct(StringComparer.OrdinalIgnoreCase);

    public override void Add(XeHeader header)
    {
        ArgumentNullException.ThrowIfNull(header);
        ArgumentException.ThrowIfNullOrWhiteSpace(header.Name);
        base.Add(header);
    }

    public void Add(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        base.Add(new XeHeader { Name = name, Value = value });
    }

    public string? GetHeader(string name)
    {
        return string.Join(", ", GetAll(name));
    }

    public void SetHeader(string name, params string?[]? values)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        RemoveAll(name);

        if (values is null || values.Length == 0) return;

        foreach (var value in values.Where(x => x is not null).Cast<string>()) Add(name, value);
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();

        foreach (var header in this)
            sb.AppendLine(header.ToString());

        return sb.ToString();
    }

    public abstract XeHeaderCollection Copy();
}
