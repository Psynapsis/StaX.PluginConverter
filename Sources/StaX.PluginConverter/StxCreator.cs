using System.IO.Compression;

namespace StaX.PluginConverter;

public static class StxCreator
{
    public static void Create(AllPluginPaths allPluginPaths, List<string> blockedList)
    {
        if (!Directory.Exists(allPluginPaths.SourceFolderPath))
        {
            Console.WriteLine("The specified folder does not exist.");
            return;
        }

        if (File.Exists(allPluginPaths.StxPath))
            File.Delete(allPluginPaths.StxPath);

        try
        {
            CreateStxFile(allPluginPaths, blockedList);
            Console.WriteLine("The .stx file was successfully created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateStxFile(AllPluginPaths allPluginPaths, List<string> blockedList)
    {
        PluginCreator.Create(allPluginPaths);
        using FileStream stream = new(allPluginPaths.StxPath, FileMode.Create);
        using ZipArchive archive = new(stream, ZipArchiveMode.Create);
        AddDirectoryToArchive(allPluginPaths.OutputSubPath, archive, allPluginPaths.SourceFolderName, blockedList);

        Directory.Delete(allPluginPaths.OutputSubPath, true);
    }

    private static void AddDirectoryToArchive(string sourceDirectory, ZipArchive archive, string archiveDirectory, List<string> blockedList)
    {
        foreach (string file in Directory.GetFiles(sourceDirectory))
        {
            bool valid = true;
            foreach (string blocked in blockedList)
            {
                if (!valid)
                    break;

                if (file.Contains(blocked) || file.Contains(".pdb"))
                    valid = false;
            }

            if (valid)
            {
                string entryName = Path.Combine(archiveDirectory, Path.GetFileName(file));
                archive.CreateEntryFromFile(file, entryName);
            }
        }

        foreach (string directory in Directory.GetDirectories(sourceDirectory))
        {
            string directoryName = Path.Combine(archiveDirectory, Path.GetFileName(directory));
            AddDirectoryToArchive(directory, archive, directoryName, blockedList);
        }
    }
}