using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
public partial class KernelFactory
{
    public Kernel BuildKernel(ILogger logger, Action<IKernelBuilder>? configure = null)
    {
        var builder = Kernel.CreateBuilder()
            .WithCompletionService(logger);
        configure?.Invoke(builder);
        return builder.Build();
    }
}
