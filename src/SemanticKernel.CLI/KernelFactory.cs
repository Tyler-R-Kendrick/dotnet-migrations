using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
namespace SKCLI;

#pragma warning disable CA2007
internal static class KernelFactory
{
    internal static IKernel BuildKernel(KernelSettings settings, ILogger logger) 
    {
        IKernel kernel = new KernelBuilder()
            .WithLogger(logger)
            .WithCompletionService(settings)
            .Build();
        return kernel;
    }
}
