namespace StaX.PluginConverter;

class Program
{
    private static List<string> _blockedList =
        [
            //"Avalonia.Skia.dll",
            //"DynamicData.dll",
            //"FluentAvalonia.dll",
            //"HarfBuzzSharp.dll",
            //"MicroCom.Runtime.dll",
            //"ReactiveUI.dll",
            //"ReactiveUI.Validation.dll",
            //"SkiaSharp.dll",
            //"Splat.dll",
            //"System.Reactive.dll",
            //"Stxs.Domain.dll",
            //".deps.json",
            //"libHarfBuzzSharp.so",
            //"libSkiaSharp.so",
            //"libHarfBuzzSharp.so",
            //"libSkiaSharp.so",
            //"libHarfBuzzSharp.so",
            //"libSkiaSharp.so",
            //"libHarfBuzzSharp.so",
            //"libSkiaSharp.so",
            //"libHarfBuzzSharp.dylib",
            //"libSkiaSharp.dylib",
            //"libHarfBuzzSharp.dll",
            //"libSkiaSharp.dll",
            //"libHarfBuzzSharp.dll",
            //"libSkiaSharp.dll",
            //"libHarfBuzzSharp.dll",
            //"libSkiaSharp.dll",
            //"Avalonia.Base.dll",
            //"Avalonia.Controls.ColorPicker.dll",
            //"Avalonia.Controls.DataGrid.dll",
            //"Avalonia.Controls.dll",
            //"Avalonia.Controls.ItemsRepeater.dll",
            //"Avalonia.DesignerSupport.dll",
            //"Avalonia.Dialogs.dll",
            //"Avalonia.dll",
            //"Avalonia.Markup.dll",
            //"Avalonia.Markup.Xaml.dll",
            //"Avalonia.Metal.dll",
            //"Avalonia.MicroCom.dll",
            //"Avalonia.OpenGL.dll",
            //"Avalonia.Remote.Protocol.dll",
        ];

    private static string _blackListPath = string.Empty;

    static void Main(string[] args)
    {
        string outputPath = string.Empty;
        var operations = args.ParceOperations();

        if (operations.Count == 0 && args.Length != 1)
        {
            Console.WriteLine(

@"Usage: <plugin folder path>
            --CreateStarter or -s <= for header file creation <folder path>
            --CreatePlugin or -p <= plugin folder creation <folder path>
            --CreateStx or -x <= create stx <folder path>
            --Output or -o <= Set Output path <folder path>
            --BlockedList or -b <= Set Black lost dlls json path <folder path>
            
            example:
                -s C:\your\folder\path -o C:\your\output\folder\path"

            );
            //--PublishPlugin or - pp <= publish project<csproj path>
            //--PublishStx or - px <= publish project && create stx<csproj path>
            return;
        }
        else if (args.Length == 1)
        {

            var allPluginPaths = new AllPluginPaths(args[0], args[0]);
            StxCreator.Create(allPluginPaths, _blockedList);
        }

        var outputJob = operations.Where(x => x.Operation is Operations.OutputPath).FirstOrDefault();
        if (outputJob is not null)
            outputPath = outputJob.Path;

        var blackListJob = operations.Where(x => x.Operation is Operations.BlackList).FirstOrDefault();
        if (blackListJob is not null)
            _blackListPath = blackListJob.Path;

        var jobs = operations.Where(x => x.Operation is not Operations.OutputPath);

        foreach (var job in jobs)
        {
            if (string.IsNullOrEmpty(outputPath))
                outputPath = job.Path;

            var allPluginPaths = new AllPluginPaths(job.Path, outputPath);
            switch (job.Operation)
            {
                case Operations.CreateStarter:
                    StarterCreator.Create(allPluginPaths);
                    break;
                case Operations.CreatePlugin:
                    PluginCreator.Create(allPluginPaths);
                    break;
                case Operations.CreateStx:
                    StxCreator.Create(allPluginPaths, _blockedList);
                    break;
                case Operations.PublishPlugin:
                    break;
                case Operations.PublishStx:
                    break;
            }
        }
    }
}