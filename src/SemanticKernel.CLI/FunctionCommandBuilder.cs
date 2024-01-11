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
            if(contextVariables.ContainsKey(parameter.Name))
                continue;
            else contextVariables.TryAdd(parameter.Name,
                await AssignInputVariable(_writer, _reader, parameter.Name, parameter.Description)
                    .ConfigureAwait(false)
                ?? parameter.DefaultValue ?? string.Empty);
        }
        return contextVariables;
    }

    private static async Task<string> AssignInputVariable(
        TextWriter _writer,
        TextReader _reader,
        string name,
        string? description)
    {
            await _writer.WriteLineAsync(description)
                .ConfigureAwait(false);
            await _writer.WriteAsync(name + ": ")
                .ConfigureAwait(false);
        var value = await _reader.ReadLineAsync().ConfigureAwait(false);
        return value ?? string.Empty;
    }
}
