namespace XeHttpTool.Tools;

internal static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo is null) return string.Empty;
        
        var attribute = (System.ComponentModel.DescriptionAttribute?)fieldInfo
            .GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
            .FirstOrDefault();

        return attribute?.Description ?? value.ToString();
    }
}
