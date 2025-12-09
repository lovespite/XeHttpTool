using System.Text.Json.Serialization;

namespace XeHttpTool.Model;

internal class XeCollection() : XeNamedEntity
{

    private readonly List<XeRequest> m_Requests = [];

    public List<XeRequest> Requests
    {
        get => m_Requests;
        set
        {
            m_Requests.Clear();
            m_Requests.AddRange(value);
        }
    }

    public string Id { get; set; } = string.Empty;
    public XeEnvironment Environment { get; set; } = new();

    public XeRequest NewRequest(string name)
    {
        var request = new XeRequest()
        {
            Name = name,
            Id = Guid.NewGuid().ToString("N"),
        };
        Requests.Add(request);
        return request;
    }

    public override string ToString()
    {
        return "XeCollection: " + Name;
    }

    public XeCollection Copy()
    {
        var copy = new XeCollection()
        {
            Name = Name + " copy",
            Description = Description,
            Environment = Environment.Copy(),
            Requests = Requests.ConvertAll(r => r.Copy()),
        }; 
        return copy;
    }
}
