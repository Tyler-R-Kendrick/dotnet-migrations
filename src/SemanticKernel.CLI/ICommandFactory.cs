using System.CommandLine;

namespace SKCLI;

/// <summary>
/// Represents a factory for creating commands.
/// </summary>
public interface ICommandFactory
{
    /// <summary>
    /// Creates a command with the specified name, description, handler, and options.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="handler">The async task representing the command's handler.</param>
    /// <param name="options">The options associated with the command.</param>
    /// <returns>The created command.</returns>
    Command Create(string name, string description, Func<Task> handler, IEnumerable<Option> options)
    {
        var command = new Command(name, description);
        foreach (var option in options)
        {
            command.AddOption(option);
        }
        command.SetHandler(handler);
        return command;
    }
}
