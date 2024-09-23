namespace StaX.PluginConverter;

public static class PluginCreator
{
    public static void Create(AllPluginPaths allPluginPaths)
    {
        if (!Directory.Exists(allPluginPaths.SourceFolderPath))
        {
            Console.WriteLine("The specified folder does not exist.");
            return;
        }

        try
        {
            CreatePluginFolder(allPluginPaths);
            Console.WriteLine("The plugin dirrectory was successfully created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreatePluginFolder(AllPluginPaths allPluginPaths)
    {
        if (Directory.Exists(allPluginPaths.BufferPath))
            Directory.Delete(allPluginPaths.BufferPath, true);
        CopyDirectory(allPluginPaths.SourceFolderPath, allPluginPaths.BufferPath);

        if (!Directory.Exists(allPluginPaths.OutputSubPath))
            Directory.CreateDirectory(allPluginPaths.OutputSubPath);

        if (Directory.Exists(allPluginPaths.OutputLastPath))
            Directory.Delete(allPluginPaths.OutputLastPath, true);

        Directory.Move(allPluginPaths.BufferPath, allPluginPaths.OutputLastPath);

        StarterCreator.Create(allPluginPaths);
    }

    private static void CopyDirectory(string sourceDir, string destinationDir)
    {
        if (!Directory.Exists(destinationDir))
            Directory.CreateDirectory(destinationDir);

        foreach (string filePath in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(filePath);
            string destFilePath = Path.Combine(destinationDir, fileName);
            File.Copy(filePath, destFilePath, true);
        }

        foreach (string dirPath in Directory.GetDirectories(sourceDir))
        {
            string dirName = Path.GetFileName(dirPath);
            string destDirPath = Path.Combine(destinationDir, dirName);
            CopyDirectory(dirPath, destDirPath);
        }
    }
}