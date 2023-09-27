using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System.Linq;
using System.Diagnostics.Contracts;

namespace SKCLI;

#pragma warning disable CS3001
/// <summary>
/// This class is responsible for generating commands
/// </summary>
public static class CommandFactory
{
    /// <summary>
    /// Builds a single parameter for a command.
    /// </summary>
    /// <param name="parameterView"></param>
    /// <returns></returns>
    public static Option BuildSkillCommandParameter(ParameterView parameterView)
    {
        Contract.Assert(parameterView is not null);
        return new Option<string?>($"--{parameterView.Name}",
                    () => parameterView.DefaultValue,
                    parameterView.Description);
    }

    /// <summary>
    /// Builds a single command from a skill.
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static Command BuildSkillCommand(
        ISKFunction skill,
        Action<SKContext> onExecute) 
    {
        Contract.Assert(skill is not null);
        var command = new Command($"{skill.Name}", skill.Description);
        var parameters = skill.Describe().Parameters;
        foreach(var @param in parameters)
        {
            var option = new Option<string?>($"--{@param.Name}",
                () => @param.DefaultValue,
                @param.Description);
            command.AddOption(option);
        }
        command.SetHandler(async () =>
        {
            var result = await skill.InvokeAsync().ConfigureAwait(false);
            onExecute(result);
        });
        return command;
    }

    /// <summary>
    /// Build all commands from skills in a directory.
    /// </summary>
    /// <param name="kernel"></param>
    /// <param name="directoryInfo"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static IEnumerable<Command> BuildSkillCommands(
        IKernel kernel,
        DirectoryInfo directoryInfo,
        Action<SKContext> onExecute) 
    {
        Contract.Assert(directoryInfo != null);
        foreach(var directory in directoryInfo.GetDirectories())
        {
            var skills = kernel.ImportSemanticSkillFromDirectory(
                directoryInfo.Name,
                directory.Name);
            foreach(var (key, skill) in skills)
            {
                yield return BuildSkillCommand(skill, onExecute);
            }
        }
    }

    /// <summary>
    /// Build a root command.
    /// </summary>
    /// <param name="onExecute"></param>
    /// <param name="kernelSettings"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static RootCommand BuildRootCommand(
        Action<SKContext> onExecute,
        KernelSettings kernelSettings,
        ILogger logger)
    {
        var rootCommand = new RootCommand("Execute skills with the semantic kernal.")
        {
            Name = "semker"
        };
        rootCommand.AddOption(new Option<string?>(
            name: "--skill",
            description: "The skill to execute.")
        {
            IsRequired = true
        });

        var skillDirInfo = GetSkillDirectory();
        Contract.Assert(skillDirInfo.Exists);
        var kernel = KernelFactory.BuildKernel(kernelSettings, logger);
        var subCommands = BuildSkillCommands(kernel, skillDirInfo, onExecute);
        foreach(var subCommand in subCommands)
        {
            if(rootCommand.Contains(subCommand)) continue;
            rootCommand.Add(subCommand);
        }
        return rootCommand;
    }

    private static DirectoryInfo GetSkillDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var skillsDirectory = Path.Combine(currentDirectory, "skills");
        return new DirectoryInfo(skillsDirectory);
    }
}