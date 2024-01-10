using System.CommandLine;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

/// <summary>
/// Represents a factory for creating commands with specified functions and execution actions.
/// </summary>
public interface IFunctionCommandFactory
{
    /// <summary>
    /// Creates a command with the specified function and execution action.
    /// </summary>
    /// <param name="skFunction">The ISKFunction instance.</param>
    /// <param name="onExecute">The action to be executed when the command is executed.</param>
    /// <returns>The created command.</returns>
    Command Create(
        ISKFunction skFunction,
        Action<SKContext> onExecute);

    /// <summary>
    /// Creates a command based on the specified parameters.
    /// </summary>
    /// <param name="onExecute">The action to be executed when the command is executed.</param>
    /// <param name="grouping">The grouping of key-value pairs representing the command.</param>
    /// <returns>The created command.</returns>
    Command Create(
        Action<SKContext> onExecute,
        IGrouping<string, KeyValuePair<string, ISKFunction>> grouping);
}
