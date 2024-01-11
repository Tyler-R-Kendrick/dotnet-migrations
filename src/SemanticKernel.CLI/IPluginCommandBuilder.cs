using System.CommandLine;
using Microsoft.SemanticKernel;

namespace SKCLI;

internal interface IPluginCommandBuilder
{
    Command BuildPluginCommand(IKernel kernel);
}
