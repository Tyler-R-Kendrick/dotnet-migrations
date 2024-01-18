using System.CommandLine;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal sealed partial class RootCommandFactory
{
    public RootCommand Create(string name,
        string? description = default,
        bool treatUnmatchedTokensAsErrors = false,
        Argument[]? arguments = default,
        Option[]? options = default,
        Command[]? subcommands = default)
    {
        description ??= "";
        arguments ??= [];
        options ??= [];
        subcommands ??= [];
        var command = new RootCommand(description)
        {
            Name = name,
            TreatUnmatchedTokensAsErrors = treatUnmatchedTokensAsErrors,
        };
        foreach (var argument in arguments)
        {
            command.AddArgument(argument);
        }
        foreach (var option in options)
        {
            command.AddOption(option);
        }
        foreach (var subcommand in subcommands)
        {
            command.Add(subcommand);
        }
        return command;
    }
}