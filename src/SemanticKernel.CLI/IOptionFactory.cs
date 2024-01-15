using System.CommandLine;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

/// <summary>
/// Represents a factory for creating options based on an ISKFunction.
/// </summary>
public interface IOptionFactory
{
    /// <summary>
    /// Creates an option with the specified aliases and arity.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="aliases">The aliases for the option.</param>
    /// <param name="arity">The arity of the option.</param>
    /// <returns>The created option.</returns>
    Option<T> CreateOption<T>(string[] aliases, ArgumentArity? arity = default);

    /// <summary>
    /// Creates a collection of options based on the provided ISKFunction.
    /// </summary>
    /// <param name="skFunction">The ISKFunction to create options for.</param>
    /// <returns>A collection of options.</returns>
    IEnumerable<Option> CreateOptions(ISKFunction skFunction);

    /// <summary>
    /// Creates an option based on the provided parameter view.
    /// </summary>
    /// <param name="parameterView">The parameter view containing the option's parameters.</param>
    /// <returns>The created option.</returns>
    Option CreateOption(ParameterView parameterView);
}
