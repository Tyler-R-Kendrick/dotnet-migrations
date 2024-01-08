using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
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
