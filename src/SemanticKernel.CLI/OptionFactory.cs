using System.CommandLine;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;
#pragma warning disable CS3001
static class OptionFactory
{
    internal static IEnumerable<Option> CreateOptions(ISKFunction function)
        => function
            .Describe()
            .Parameters
            .Select(CreateOption);

    internal static Option CreateOption(ParameterView parameterView)
        => new Option<string?>($"--{parameterView.Name}",
            () => parameterView.DefaultValue,
            parameterView.Description);
}
