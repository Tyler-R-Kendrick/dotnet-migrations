using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
public partial class KernelFactory
{
    public Kernel BuildKernel()
    => Kernel.CreateBuilder()
        .WithCompletionService()
        .Build();
}
