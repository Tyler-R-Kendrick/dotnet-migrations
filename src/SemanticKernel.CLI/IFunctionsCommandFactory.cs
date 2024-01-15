using System.CommandLine;
using Microsoft.SemanticKernel;

namespace SKCLI;

internal interface IFunctionsCommandFactory
{
    Command BuildFunctionsCommand(IKernel kernel, IEnumerable<string> plugins);
}
