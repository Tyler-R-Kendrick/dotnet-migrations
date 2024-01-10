using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

/// <summary>
/// Represents a factory for creating root commands.
/// </summary>
public interface IRootCommandFactory
{
    /// <summary>
    /// Builds a root command with the specified action and kernel.
    /// </summary>
    /// <param name="onExecute">The action to be executed when the root command is executed.</param>
    /// <param name="kernel">The kernel to be used by the root command.</param>
    /// <returns>The built root command.</returns>
    RootCommand BuildRootCommand(Action<SKContext> onExecute, IKernel kernel);
    
    /// <summary>
    /// Builds the plugin command and adds it to the root command.
    /// </summary>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="rootCommand">The root command to add the plugin command to.</param>
    void BuildPluginCommand(IKernel kernel, RootCommand rootCommand);

    /// <summary>
    /// Builds the plugins option for the specified kernel and root command.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="rootCommand">The root command.</param>
    void BuildPluginsOption(IKernel kernel, RootCommand rootCommand);

    /// <summary>
    /// Retrieves a dictionary of function views associated with the specified kernel.
    /// </summary>
    /// <param name="kernel">The kernel to retrieve function views from.</param>
    /// <returns>A dictionary containing function views grouped by their names.</returns>
    IDictionary<string, IEnumerable<FunctionView>> GetFunctionViews(IKernel kernel);

    /// <summary>
    /// Retrieves a collection of registered functions from the specified kernel.
    /// </summary>
    /// <param name="kernel">The kernel to retrieve registered functions from.</param>
    /// <returns>A collection of registered functions.</returns>
    IEnumerable<string> GetRegisteredFunctions(IKernel kernel);

    /// <summary>
    /// Retrieves the registered functions from the specified kernel and plugin.
    /// </summary>
    /// <param name="kernel">The kernel to retrieve the functions from.</param>
    /// <param name="plugin">The plugin to filter the functions by.</param>
    /// <returns>An enumerable collection of registered functions.</returns>
    IEnumerable<string> GetRegisteredFunctions(IKernel kernel, string plugin);

    /// <summary>
    /// Retrieves the list of registered plugins from the specified kernel.
    /// </summary>
    /// <param name="kernel">The kernel from which to retrieve the registered plugins.</param>
    /// <returns>An enumerable collection of registered plugin names.</returns>
    IEnumerable<string> GetRegisteredPlugins(IKernel kernel);

    /// <summary>
    /// Builds the root subcommands for the CLI application.
    /// </summary>
    /// <param name="onExecute">The action to be executed when a subcommand is executed.</param>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="rootCommand">The root command instance.</param>
    /// <param name="directoryInfo">The optional directory information.</param>
    void BuildRootSubcommands(
        Action<SKContext> onExecute,
        IKernel kernel,
        RootCommand rootCommand,
        DirectoryInfo? directoryInfo = null);
}
