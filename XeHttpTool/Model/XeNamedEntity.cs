namespace XeHttpTool.Model;

internal abstract class XeNamedEntity
{
    public virtual string Name { get; set; } = string.Empty;
    public virtual string? Description { get; set; } = null;
}
