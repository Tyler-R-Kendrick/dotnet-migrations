using System.CommandLine;
using Microsoft.SemanticKernel;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal partial class PluginCommandBuilder(
    ICommandFactory _commandFactory,
    IFunctionsCommandFactory _functionsCommandFactory,
    IFunctionCommandBuilder _functionCommandFactory)
    : IPluginCommandBuilder
{
    public Command BuildPluginCommand(IKernel kernel)
    {
        var plugins = kernel.GetRegisteredPlugins();
        var functionsCommand = _functionsCommandFactory.BuildFunctionsCommand(kernel, plugins);
        var pluginArgument = new Argument<string>("name")
            .FromAmong(plugins.ToArray());
        var functionCommand = _functionCommandFactory.BuildFunctionCommand(kernel, pluginArgument);
        var command = _commandFactory.Create("--plugin",
            () => Task.CompletedTask,
            ["-p"], "Select a plugin",
            arguments: [pluginArgument],
            commands: [functionsCommand, functionCommand]);
        return command;
    }
}
