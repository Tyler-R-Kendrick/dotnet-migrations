using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
namespace SKCLI;

internal class KernelFactory : IKernelFactory
{
    public IKernel BuildKernel(KernelSettings settings, ILogger logger)
    => new KernelBuilder()
        .WithLogger(logger)
        .WithCompletionService(settings)
        .Build();
}
