using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using System.Diagnostics.Contracts;

namespace SKCLI;

internal class RootCommandFactory(
    IPluginCommandFactory _pluginCommandFactory,
    IPluginCommandBuilder _pluginCommandBuilder,
    TextWriter _writer)
    : IRootCommandFactory
{
    public RootCommand BuildRootCommand(
        Action<SKContext> onExecute,
        IKernel kernel)
    {
        var rootCommand = new RootCommand("Execute plugins with the semantic kernal.")
        {
            Name = "semker",
            TreatUnmatchedTokensAsErrors = false
        };
        BuildRootSubcommands(onExecute, kernel, rootCommand);
        BuildPluginsOption(kernel, rootCommand);
        BuildPluginCommand(kernel, rootCommand);
        return rootCommand;
    }

    private void BuildPluginCommand(IKernel kernel, RootCommand rootCommand)
    {
        var pluginCommand = _pluginCommandBuilder.BuildPluginCommand(kernel);
        rootCommand.Add(pluginCommand);
    }

    internal void BuildPluginsOption(IKernel kernel, RootCommand rootCommand)
    {
        string[] aliases = ["--plugins", "-P"];
        var option = new Option<bool?>(aliases)
        {
            Arity = ArgumentArity.Zero
        };
        rootCommand.AddOption(option);
        rootCommand.SetHandler(async (options) =>
        {
            foreach (var key in kernel.GetRegisteredPlugins())
            {
                await _writer.WriteLineAsync(key)
                    .ConfigureAwait(false);
            }
        }, option);
    }

    internal void BuildRootSubcommands(
        Action<SKContext> onExecute,
        IKernel kernel,
        RootCommand rootCommand,
        DirectoryInfo? directoryInfo = null)
    {
        var skillDirInfo = directoryInfo ?? _pluginCommandFactory.GetPluginsDirectory();
        Contract.Assert(skillDirInfo.Exists);
        var subCommands = _pluginCommandFactory.CreateCommands(kernel, skillDirInfo, onExecute);
        foreach (var subCommand in subCommands)
        {
            rootCommand.Add(subCommand);
        }
    }
}
