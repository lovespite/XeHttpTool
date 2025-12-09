using System.Text.Json.Serialization;

namespace XeHttpTool.Model;

internal class XeCollection() : XeNamedEntity
{
    [JsonIgnore] public XeWorkspace? Workspace { get; set; }

    private readonly List<XeRequest> m_Requests = [];

    public List<XeRequest> Requests
    {
        get => m_Requests;
        set
        {
            m_Requests.Clear();
            m_Requests.AddRange(value);
            foreach (var item in m_Requests) item.Collection = this;
        }
    }

    public XeRequest NewRequest(string name)
    {
        var request = new XeRequest { Name = name, Collection = this };
        Requests.Add(request);
        return request;
    }

    public XeEnvironment Environment { get; set; } = new();

    public XeCollection(XeWorkspace workspace) : this()
    {
        Workspace = workspace;
    }

    public override string ToString()
    {
        return "XeCollection: " + Name;
    }

    public XeCollection Copy()
    {
        var copy = new XeCollection
        {
            Name = Name + " copy",
            Description = Description,
            Environment = Environment.Copy()
        };
        foreach (var request in Requests) copy.Requests.Add(request.Copy());
        return copy;
    }
}
