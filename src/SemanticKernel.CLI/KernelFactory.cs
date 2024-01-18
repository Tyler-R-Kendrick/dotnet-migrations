using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal partial class KernelFactory
{
    public IKernel BuildKernel(KernelSettings settings, ILogger logger)
    => new KernelBuilder()
        .WithLogger(logger)
        .WithCompletionService(settings)
        .Build();
}
