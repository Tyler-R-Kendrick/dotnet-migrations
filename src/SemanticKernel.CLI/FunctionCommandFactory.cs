using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

static class FunctionCommandFactory
{
    private static Command CreateCommand(
        ISKFunction function,
        Action<SKContext> onExecute) 
    {
        var command = new Command($"{function.Name}", function.Description);
        foreach (var option in OptionFactory.CreateOptions(function))
        {
            command.AddOption(option);
        }
        command.SetHandler(async () =>
        {
            var result = await function
                .InvokeAsync()
                .ConfigureAwait(false);
            onExecute(result);
        });
        return command;
    }

    internal static Command CreateCommands(
        Action<SKContext> onExecute,
        IGrouping<string, KeyValuePair<string, ISKFunction>> grouping)
    {
        var skillCommand = new Command(grouping.Key);
        foreach (var (key, function) in grouping)
        {
            var subCommand = CreateCommand(function, onExecute);
            skillCommand.AddCommand(subCommand);
        }
        return skillCommand;
    }
}