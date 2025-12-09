namespace XeHttpTool.Model;

internal static class XeWorkspaceExtensions
{
    #region Serialization 

    public static void Save(this XeWorkspace workspace)
    {
        if (workspace.SourceFilePath is null)
        {
            throw new InvalidOperationException("The workspace does not have a file source path specified.");
        }
        SaveTo(workspace, workspace.SourceFilePath);
    }

    public static void SaveTo(this XeWorkspace workspace, string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        using var fs = File.Create(filePath);
        SaveTo(workspace, fs);
    }

    public static void SaveTo(this XeWorkspace workspace, Stream stream)
    {
        System.Text.Json.JsonSerializer.Serialize(stream, workspace, Serialization.XeModelJsonContext.Default.XeWorkspace);
    }

    public static XeWorkspace ReadAsXeWorkspace(this Stream fs, string? sourceFilePath = null)
    {
        var workspace = System.Text.Json.JsonSerializer.Deserialize(fs, Serialization.XeModelJsonContext.Default.XeWorkspace)
             ?? throw new InvalidDataException("Failed to deserialize XeWorkspace from the provided file.");

        workspace.SourceFilePath = sourceFilePath ?? (fs as FileStream)?.Name;
        return workspace;
    }

    public static XeWorkspace ReadAsXeWorkspace(this FileInfo file)
    {
        using var fs = file.OpenRead();
        return ReadAsXeWorkspace(fs);
    }


    public static async Task SaveAsync(this XeWorkspace workspace, CancellationToken token = default)
    {
        if (workspace.SourceFilePath is null)
        {
            throw new InvalidOperationException("The workspace does not have a file source path specified.");
        }
        await SaveToAsync(workspace, workspace.SourceFilePath, token);
    }

    public static async Task SaveToAsync(this XeWorkspace workspace, string filePath, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        using var fs = File.Create(filePath);
        await SaveToAsync(workspace, fs, token);
    }

    public static async Task SaveToAsync(this XeWorkspace workspace, Stream stream, CancellationToken token = default)
    {
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, workspace, Serialization.XeModelJsonContext.Default.XeWorkspace, token);
    }

    public static async Task<XeWorkspace> ReadAsXeWorkspaceAsync(this Stream stream, string? sourceFilePath = null, CancellationToken token = default)
    {
        var workspace = await System.Text.Json.JsonSerializer.DeserializeAsync(stream, Serialization.XeModelJsonContext.Default.XeWorkspace, token)
             ?? throw new InvalidDataException("Failed to deserialize XeWorkspace from the provided stream.");

        workspace.SourceFilePath = sourceFilePath ?? (stream as FileStream)?.Name;
        return workspace;
    }

    public static async Task<XeWorkspace> ReadAsXeWorkspace(this FileInfo file, CancellationToken token = default)
    {
        using var fs = File.OpenRead(file.FullName);

        return await ReadAsXeWorkspaceAsync(fs, file.FullName, token);
    }

    #endregion
}