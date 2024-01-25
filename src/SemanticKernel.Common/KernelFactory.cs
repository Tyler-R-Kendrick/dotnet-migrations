using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
public partial class KernelFactory
{
    public Kernel BuildKernel(Action<IKernelBuilder>? configure = null)
    {
        var builder = Kernel.CreateBuilder();
        configure?.Invoke(builder);
        return builder.Build();
    }
}
