namespace XeHttpTool.Model;

internal abstract class XeNamedCollection<TItem> : ICollection<TItem> where TItem : XeNamedEntity, new()
{
    protected readonly List<TItem> m_Items = [];

    public virtual TItem New(string name)
    {
        ThrowIfReadOnly();
        var item = new TItem
        {
            Name = name
        };
        Add(item);
        return item;
    }

    public virtual TItem? this[string name]
    {
        get => Get(name);
        set
        {
            if (value is null) Remove(name);
            else Set(name, value);
        }
    }

    public int Count => m_Items.Count;

    public virtual bool IsReadOnly => false;

    protected void ThrowIfReadOnly()
    {
        if (IsReadOnly) throw new InvalidOperationException("无法修改只读集合");
    }

    public virtual void Add(TItem item)
    {
        ThrowIfReadOnly();
        ArgumentNullException.ThrowIfNull(item);
        m_Items.Add(item);
    }

    public void Clear()
    {
        ThrowIfReadOnly();
        m_Items.Clear();
    }

    public bool Contains(TItem item)
    {
        return m_Items.Contains(item);
    }

    public bool Contains(string name)
    {
        return m_Items.Exists(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public void CopyTo(TItem[] array, int arrayIndex)
    {
        m_Items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<TItem> GetEnumerator()
    {
        return m_Items.GetEnumerator();
    }

    public bool Remove(TItem item)
    {
        ThrowIfReadOnly();
        return m_Items.Remove(item);
    }

    public bool Remove(string name)
    {
        ThrowIfReadOnly();

        for (var index = m_Items.Count - 1; index >= 0; --index)
        {
            var item = m_Items[index];
            if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                m_Items.RemoveAt(index);
                return true;
            }
        }

        return false;
    }

    public int RemoveAll(string name)
    {
        ThrowIfReadOnly();
        return m_Items.RemoveAll(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public TItem? Get(string name)
    {
        var index = m_Items.FindIndex(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return null;
        return m_Items[index];
    }

    public TItem[] GetAll(string name)
    {
        return [.. m_Items.Where(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase))];
    }

    public void Set(string name, TItem item)
    {
        ThrowIfReadOnly();
        var index = m_Items.FindIndex(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            m_Items.Add(item);
        }
        else
        {
            m_Items[index] = item;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return ((System.Collections.IEnumerable)m_Items).GetEnumerator();
    }
}
