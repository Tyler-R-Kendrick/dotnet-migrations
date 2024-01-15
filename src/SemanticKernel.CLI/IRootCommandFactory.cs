using System.CommandLine;

namespace SKCLI;

internal interface IRootCommandFactory
{
    RootCommand Create(string name,
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

internal class RootCommandFactory : IRootCommandFactory
{
}