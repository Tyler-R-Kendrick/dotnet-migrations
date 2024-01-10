using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

// Define the interface for PluginCommandFactory
internal interface IPluginCommandFactory
{
    DirectoryInfo GetPluginsDirectory();
    IEnumerable<KeyValuePair<string, ISKFunction>> GetDirectoryPlugins(IKernel kernel, DirectoryInfo directoryInfo);
    IEnumerable<Command> CreateCommands(IKernel kernel, DirectoryInfo directoryInfo, Action<SKContext> onExecute);
}
