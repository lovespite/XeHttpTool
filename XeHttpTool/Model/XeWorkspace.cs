using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace XeHttpTool.Model;

/*

Structure:

Workspace
  - Environment
  - Collections
    - Environment (overrides workspace environment)
    - Request
      - Scripts
*/

internal class XeWorkspace : XeNamedEntity
{
    private readonly List<XeCollection> m_Collections = [];

    public List<XeCollection> Collections
    {
        get => m_Collections;
        set
        {
            m_Collections.Clear();
            m_Collections.AddRange(value);
        }
    }

    public string Id { get; set; } = string.Empty;
    public XeEnvironment Environment { get; set; } = new();
    public ushort Version { get; set; } = 0x0100;
    [JsonIgnore] public string? SourceFilePath { get; set; }

    public override string ToString()
    {
        return "XeWorkspace: " + Name;
    }

    public XeCollection NewCollection(string name)
    {
        var collection = new XeCollection()
        {
            Name = name,
            Id = Guid.NewGuid().ToString("N"),
        };
        Collections.Add(collection);
        return collection;
    }

    public static async Task<XeWorkspace> CreateAsync(string name, string? saveTo = null)
    {
        var workspace = new XeWorkspace
        {
            Name = name,
            Id = Guid.NewGuid().ToString("N"),
        };

        if (!string.IsNullOrWhiteSpace(saveTo))
        {
            await workspace.SaveToAsync(saveTo);
            workspace.SourceFilePath = saveTo;
        }

        return workspace;
    }
}
