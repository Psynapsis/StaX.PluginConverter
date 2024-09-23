using FluentAvalonia.UI.Controls;
using System.Reflection;
using System.Text.Json;
using StaX.Domain;

namespace StaX.PluginConverter;

public static class StarterCreator
{
    public static void Create(AllPluginPaths allPluginPaths)
    {
        if (!Directory.Exists(allPluginPaths.SourceFolderPath))
        {
            Console.WriteLine("The specified folder does not exist.");
            return;
        }

        if (!Directory.Exists(allPluginPaths.OutputSubPath))
            Directory.CreateDirectory(allPluginPaths.OutputSubPath);

        if (File.Exists(allPluginPaths.StarterPath))
            File.Delete(allPluginPaths.StarterPath);

        try
        {
            CreateStartFile(allPluginPaths);
            Console.WriteLine("The start.json file was successfully created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateStartFile(AllPluginPaths allPluginPaths)
    {
        var starter = GetStarter(allPluginPaths.SourceFolderPath);
        if (starter is not null)
        {
            var json = JsonSerializer.Serialize(starter);
            Console.WriteLine($"{json}");
            File.WriteAllText(allPluginPaths.StarterPath, json);
        }
    }

    private static Starter? GetStarter(string path)
    {
        if (path.Contains(".dll", StringComparison.CurrentCultureIgnoreCase))
            return GetStarterFromDll<IUiState>(path);
        else
            return GetStarterFromFolder<IUiState>(path);
    }

    private static Starter? GetStarterFromDll<T>(string path)
    {
        try
        {
            var assembly = Assembly.LoadFrom(path);

            foreach (var type in assembly.GetTypes())
                if (typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var types = assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
                    foreach (var thisType in types)
                    {
                        var uiStateObject = Activator.CreateInstance(thisType);

                        var uiState = uiStateObject as IUiState;

                        if (uiState is not null)
                        {
                            Starter starter = new()
                            {
                                Implementer = Path.GetFileName(path),
                                Name = uiState.StateName,
                                ToolTip = uiState.ToolTip,
                                Symbol = uiState.Icon ?? Symbol.Accept
                            };
                            return starter;
                        }
                    }
                }
        }
        catch
        {
        }
        return null;
    }

    private static Starter? GetStarterFromFolder<T>(string path)
    {
        var dllFiles = Directory.GetFiles(path, "*.dll");

        foreach (var dllFile in dllFiles)
        {
            var starter = GetStarterFromDll<T>(dllFile);
            if (starter != null)
                return starter;
        }

        return null;
    }
}
