
using System.CommandLine;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

/// <summary>
/// Provides extension methods for the <see cref="IOptionFactory"/> interface.
/// </summary>
public static class OptionFactoryExtensions
{
    /// <summary>
    /// Creates a collection of options based on the parameters of the specified <see cref="ISKFunction"/>.
    /// </summary>
    /// <param name="optionFactory">The <see cref="IOptionFactory"/> instance.</param>
    /// <param name="skFunction">The <see cref="ISKFunction"/> instance.</param>
    /// <returns>A collection of <see cref="Option"/> objects.</returns>
    public static IEnumerable<Option> CreateOptions(this IOptionFactory optionFactory, ISKFunction skFunction)
    {
        var parameters = skFunction.Describe().Parameters;
        foreach (var parameter in parameters)
        {
            var option = optionFactory.CreateOption<string>(parameter);
            yield return option;
        }
    }

    /// <summary>
    /// Creates an <see cref="Option"/> based on the specified <see cref="ParameterView"/>.
    /// </summary>
    /// <param name="optionFactory">The <see cref="IOptionFactory"/> instance.</param>
    /// <param name="parameter">The <see cref="ParameterView"/> instance.</param>
    /// <returns>An <see cref="Option"/> object.</returns>
    public static Option<T> CreateOption<T>(this IOptionFactory optionFactory, ParameterView parameter)
        => optionFactory.CreateOption<T>(parameter.Name, parameter.Description);
}