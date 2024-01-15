using System.CommandLine;
using Microsoft.SemanticKernel;

namespace SKCLI;

internal interface IFunctionCommandBuilder
{
    Command BuildFunctionCommand(IKernel kernel, Argument<string> pluginArgument);
}
