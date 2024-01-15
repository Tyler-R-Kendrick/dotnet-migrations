using System.CommandLine;
using Microsoft.SemanticKernel;

namespace SKCLI;

internal class FunctionsCommandFactory(
    TextWriter _writer)
    : IFunctionsCommandFactory
{
    public Command BuildFunctionsCommand(IKernel kernel, IEnumerable<string> plugins)
    {
        var functionsCommand = new Command("--functions", "list all functions for a plugin.");
        functionsCommand.AddAlias("-F");
        var functionsArgument = new Argument<string>("name")
            .FromAmong(plugins.ToArray());
        functionsCommand.SetHandler(async (plugin) =>
        {
            foreach (var function in kernel.GetRegisteredFunctions(plugin))
            {
                await _writer.WriteLineAsync(function)
                    .ConfigureAwait(false);
            }
        }, functionsArgument);
        return functionsCommand;
    }
}
