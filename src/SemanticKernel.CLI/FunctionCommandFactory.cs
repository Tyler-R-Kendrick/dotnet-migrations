using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal partial class FunctionCommandFactory(
    ICommandFactory _commandFactory,
    IOptionFactory _optionFactory)
    : IFunctionCommandFactory
{
    public Command Create(
        ISKFunction skFunction,
        Action<SKContext> onExecute)
        => _commandFactory.Create(
            skFunction.Name,
            async () =>
            {
                var result = await skFunction
                    .InvokeAsync()
                    .ConfigureAwait(false);
                onExecute(result);
            },
            description: skFunction.Description,
            options: _optionFactory.CreateOptions(skFunction));

    public Command Create(
        Action<SKContext> onExecute,
        IGrouping<string, KeyValuePair<string, ISKFunction>> grouping)
    {
        var skillCommand = _commandFactory.Create(grouping.Key);
        foreach (var (key, function) in grouping)
        {
            var subCommand = Create(function, onExecute);
            skillCommand.AddCommand(subCommand);
        }
        return skillCommand;
    }
}