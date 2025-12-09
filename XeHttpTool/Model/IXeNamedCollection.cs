namespace XeHttpTool.Model;

internal interface IXeNamedCollection<TItem> where TItem : XeNamedEntity
{
    public TItem? Get(string name);
    public void Set(string name, TItem item);
    public bool Remove(string name);
}
