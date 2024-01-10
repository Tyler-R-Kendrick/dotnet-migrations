using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
namespace SKCLI;

internal interface IKernelFactory
{
    IKernel BuildKernel(KernelSettings settings, ILogger logger);
}
