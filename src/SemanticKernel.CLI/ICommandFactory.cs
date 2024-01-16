using System.CommandLine;

namespace SKCLI;

/// <summary>
/// Represents a factory for creating commands.
/// </summary>
internal interface ICommandFactory
{
    Command Create(string name, string description) => new Command(name, description);

    /// <summary>
    /// Creates a command with the specified name, description, handler, and options.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="aliases"></param>
    /// <param name="handler">The async task representing the command's handler.</param>
    /// <param name="options">The options associated with the command.</param>
    /// <param name="arguments"></param>
    /// <param name="commands"></param>
    /// <returns>The created command.</returns>
    Command Create(string name,
        Func<Task> handler,
        string[]? aliases = default,
        string? description = default,
        IEnumerable<Option>? options = default,
        IEnumerable<Argument>? arguments = default,
        IEnumerable<Command>? commands = default)
    {
        aliases ??= [];
        description ??= "";
        options ??= Array.Empty<Option>();
        arguments ??= Array.Empty<Argument>();
        commands ??= Array.Empty<Command>();
        var command = Create(name, description);
        foreach (var alias in aliases)
        {
            command.AddAlias(alias);
        }
        foreach (var option in options)
        {
            command.AddOption(option);
        }
        foreach (var argument in arguments)
        {
            command.AddArgument(argument);
        }
        foreach (var subcommand in commands)
        {
            command.AddCommand(subcommand);
        }
        command.SetHandler(handler);
        return command;
    }
}
