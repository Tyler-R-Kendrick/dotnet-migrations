using System.CommandLine;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

internal class OptionFactory : IOptionFactory
{
    public IEnumerable<Option> CreateOptions(ISKFunction function)
        => function
            .Describe()
            .Parameters
            .Select(CreateOption);

    public Option CreateOption(ParameterView parameterView)
        => new Option<string?>($"--{parameterView.Name}",
            () => parameterView.DefaultValue,
            parameterView.Description);
}
