using System.Text.Json.Serialization;

namespace XeHttpTool.Model.Serialization;

[JsonSerializable(typeof(XeWorkspace))]
[JsonSerializable(typeof(XeCollection))]
[JsonSerializable(typeof(XeRequest))]
[JsonSerializable(typeof(XeRequestBody))]
[JsonSerializable(typeof(XeHeader))]
[JsonSerializable(typeof(XeRequestHeaderCollection))]
[JsonSerializable(typeof(XeResponseHeaderCollection))]
[JsonSerializable(typeof(XeEnvironment))]
[JsonSerializable(typeof(XeScripts))]
[JsonSourceGenerationOptions(WriteIndented = false,
                             UseStringEnumConverter = true,
                             DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                             PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class XeModelJsonContext : JsonSerializerContext
{
}
