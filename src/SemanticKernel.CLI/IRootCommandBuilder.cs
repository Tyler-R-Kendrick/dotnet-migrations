using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;

namespace SKCLI;

/// <summary>
/// Represents a builder for creating root commands.
/// </summary>
public interface IRootCommandBuilder
{
    /// <summary>
    /// Builds a root command with the specified action and kernel.
    /// </summary>
    /// <param name="onExecute">The action to be executed when the root command is executed.</param>
    /// <param name="kernel">The kernel to be used by the root command.</param>
    /// <returns>The built root command.</returns>
    RootCommand BuildRootCommand(Action<SKContext> onExecute, IKernel kernel);
}
