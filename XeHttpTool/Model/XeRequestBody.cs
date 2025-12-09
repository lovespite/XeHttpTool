namespace XeHttpTool.Model;

internal class XeRequestBody
{
    public bool IsEmpty => BodyType == XeRequestBodyRawType.Empty;
    public string? TextualRepresentation { get; set; }
    public XeRequestBodyRawType BodyType { get; set; } = XeRequestBodyRawType.Empty;
    public XeRequestBodyRawFormat Format { get; set; } = XeRequestBodyRawFormat.None;

    public XeRequestBody Copy()
    {
        return new XeRequestBody
        {
            TextualRepresentation = TextualRepresentation,
            BodyType = BodyType,
            Format = Format,
        };
    }

    public override string ToString() => TextualRepresentation ?? "<Empty>";

    public static readonly XeRequestBody Empty = new();
}
