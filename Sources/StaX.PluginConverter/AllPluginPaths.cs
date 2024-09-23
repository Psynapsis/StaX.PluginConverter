namespace StaX.PluginConverter;

public record AllPluginPaths(string SourceFolderPath, string OutputTopPath)
{
    public string SourceFolderName => Path.GetFileName(SourceFolderPath);

    public string OutputSubPath => Path.Combine(OutputTopPath, SourceFolderName);

    public string BufferPath => Path.Combine(Path.GetDirectoryName(SourceFolderPath)!, "BufferPluginCreator");

    public string OutputLastPath => Path.Combine(OutputSubPath, "Plugin");

    public string StxPath => OutputSubPath + ".stx";

    public string StarterPath => Path.Combine(OutputSubPath, "start.json");
}