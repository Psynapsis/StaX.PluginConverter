namespace StaX.PluginConverter;

public static class CommandParcer
{
    public static List<OperationParameter> ParceOperations(this string[] args)
    {
        List<OperationParameter> operationsParameters = [];

        try
        {
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length - 1; i++)
                {
                    var operation = args[i] switch
                    {
                        "--CreateStarter" => Operations.CreateStarter,
                        "-s" => Operations.CreateStarter,
                        "--CreatePlugin" => Operations.CreatePlugin,
                        "-p" => Operations.CreatePlugin,
                        "--CreateStx" => Operations.CreateStx,
                        "-x" => Operations.CreateStx,
                        "--PublishPlugin" => Operations.PublishPlugin,
                        "-pp" => Operations.PublishPlugin,
                        "--PublishStx" => Operations.PublishStx,
                        "-px" => Operations.PublishStx,
                        "--Output" => Operations.OutputPath,
                        "-o" => Operations.OutputPath,
                        "--BlackList" => Operations.BlackList,
                        "-b" => Operations.BlackList,
                        _ => Operations.None,
                    };

                    if (operation is not Operations.None)
                        operationsParameters.Add(new(operation, args[i + 1]));
                }
            }
        }
        catch (Exception)
        {
            //ignore
        }

        return operationsParameters;
    }
}
