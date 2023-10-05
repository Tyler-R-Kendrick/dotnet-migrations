using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

static class PluginCommandFactory
{
    internal static DirectoryInfo GetPluginsDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var skillsDirectory = Path.Combine(currentDirectory, "skills");
        return new DirectoryInfo(skillsDirectory);
    }
    
    internal static IEnumerable<KeyValuePair<string, ISKFunction>> GetDirectoryPlugins(
        IKernel kernel,
        DirectoryInfo directoryInfo)
        => directoryInfo
            .GetDirectories()
            .SelectMany(x =>
                kernel.ImportSemanticSkillFromDirectory(
                    directoryInfo.Name,
                    x.Name));

    internal static IEnumerable<Command> CreateCommands(
        IKernel kernel,
        DirectoryInfo directoryInfo,
        Action<SKContext> onExecute)
        => GetDirectoryPlugins(kernel, directoryInfo)
            .GroupBy(x => x.Value.SkillName)
            .Select(grouping => FunctionCommandFactory.CreateCommands(onExecute, grouping));
}
