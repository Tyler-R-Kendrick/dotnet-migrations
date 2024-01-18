using System.CommandLine;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal sealed partial class OptionFactory
{
    /// <summary>
    /// Creates an option with the specified aliases and arity.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="aliases">The aliases for the option.</param>
    /// <param name="arity">The arity of the option.</param>
    /// <returns>The created option.</returns>
    public Option<T> CreateOption<T>(string[] aliases, ArgumentArity? arity = default)
        => new(aliases) { Arity = arity ?? ArgumentArity.Zero };

    public Option<T> CreateOption<T>(string name, string? description = default, ArgumentArity? arity = default)
    {
        var option = CreateOption<T>([name], arity);
        option.Description = description;
        return option;
    }
}
