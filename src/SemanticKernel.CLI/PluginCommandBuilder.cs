using System.CommandLine;
using Microsoft.SemanticKernel;

namespace SKCLI;

internal class PluginCommandBuilder : IPluginCommandBuilder
{
    private readonly FunctionsCommandFactory functionsCommandFactory;
    private readonly FunctionCommandBuilder functionCommandFactory;

    public PluginCommandBuilder(
        FunctionsCommandFactory functionsCommandFactory,
        FunctionCommandBuilder functionCommandFactory)
    {
        this.functionsCommandFactory = functionsCommandFactory;
        this.functionCommandFactory = functionCommandFactory;
    }

    public Command BuildPluginCommand(IKernel kernel)
    {
        var plugins = kernel.GetRegisteredPlugins();
        var pluginCommand = new Command("--plugin", "Select a plugin");
        pluginCommand.AddAlias("-p");
        var pluginArgument = new Argument<string>("name")
            .FromAmong(plugins.ToArray());
        pluginCommand.AddArgument(pluginArgument);

        var functionsCommand = functionsCommandFactory.BuildFunctionsCommand(kernel, plugins);
        pluginCommand.AddCommand(functionsCommand);
        var functionCommand = functionCommandFactory.BuildFunctionCommand(kernel, pluginArgument);
        pluginCommand.AddCommand(functionCommand);

        return pluginCommand;
    }
}
