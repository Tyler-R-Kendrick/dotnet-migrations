using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

internal class FunctionCommandBuilder(
    TextWriter _writer,
    TextReader _reader)
{
    public Command BuildFunctionCommand(IKernel kernel, Argument<string> pluginArgument)
    {
        var functionCommand = new Command("--function", "The function to execute")
        {
            TreatUnmatchedTokensAsErrors = false
        };
        functionCommand.AddAlias("-f");
        functionCommand.AddArgument(pluginArgument);
        var functionArgument = new Argument<string>("name");
        //todo: add argument validator.
        functionCommand.AddArgument(functionArgument);
        //todo: add handler for dynamic function args
        functionCommand.SetHandler(async (plugin, function) =>
        {
            var skFunction = kernel.Func(plugin, function);
            var view = skFunction.Describe();
            var parameters = view.Parameters;
            var contextVariables = await BuildContextVariables(_writer, _reader, parameters)
                .ConfigureAwait(false);
            var result = await skFunction.InvokeAsync(contextVariables)
                .ConfigureAwait(false);
            await _writer.WriteLineAsync(result.Result)
                .ConfigureAwait(false);
        }, pluginArgument, functionArgument);
        return functionCommand;
    }
    private static async Task<ContextVariables> BuildContextVariables(
        TextWriter _writer,
        TextReader _reader,
        IList<ParameterView> parameters)
    {
        var contextVariables = new ContextVariables();
        foreach (var parameter in parameters)
        {
            await _writer.WriteLineAsync(parameter.Description)
                .ConfigureAwait(false);
            await _writer.WriteAsync(parameter.Name + ": ")
                .ConfigureAwait(false);
            var value = await _reader.ReadLineAsync().ConfigureAwait(false);
            contextVariables[parameter.Name] = value ?? string.Empty;
        }
        return contextVariables;
    }
}
