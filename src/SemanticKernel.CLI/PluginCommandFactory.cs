using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal partial class PluginCommandFactory(
    IFunctionCommandFactory _functionCommandFactory)
    : IPluginCommandFactory
{
    public DirectoryInfo GetPluginsDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var skillsDirectory = Path.Combine(currentDirectory, "skills");
        return new DirectoryInfo(skillsDirectory);
    }
    
    public IEnumerable<KeyValuePair<string, ISKFunction>> GetDirectoryPlugins(
        IKernel kernel,
        DirectoryInfo directoryInfo)
        => directoryInfo
            .GetDirectories()
            .SelectMany(x =>
                kernel.ImportSemanticSkillFromDirectory(
                    directoryInfo.Name,
                    x.Name));

    public IEnumerable<Command> CreateCommands(
        IKernel kernel,
        DirectoryInfo directoryInfo,
        Action<SKContext> onExecute)
        => GetDirectoryPlugins(kernel, directoryInfo)
            .GroupBy(x => x.Value.SkillName)
            .Select(grouping => _functionCommandFactory.Create(onExecute, grouping));
}
