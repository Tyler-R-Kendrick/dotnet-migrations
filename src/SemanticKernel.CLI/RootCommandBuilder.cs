using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using System.Diagnostics.Contracts;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal sealed partial class RootCommandBuilder(
    IRootCommandFactory _rootCommandFactory,
    IPluginCommandFactory _pluginCommandFactory,
    IPluginCommandBuilder _pluginCommandBuilder,
    IOptionFactory _optionFactory,
    IArgumentFactory _argumentFactory,
    TextWriter _writer)
{
    public RootCommand BuildRootCommand(
        Action<SKContext> onExecute,
        IKernel kernel)
    {
        var inputArgument = _argumentFactory.Create<string>("input");
        var rootCommand = _rootCommandFactory.Create(
            "semker",
            "Execute plugins with the semantic kernal.",
            false,
            arguments: [inputArgument],
            options: [],
            subcommands: []);
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
        var option = _optionFactory.CreateOption<bool?>(aliases);
#pragma warning disable CA2201 // Do not raise reserved exception types
        if (option == null) throw new Exception("option is null");
#pragma warning restore CA2201 // Do not raise reserved exception types
        ArgumentNullException.ThrowIfNull(rootCommand);
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
