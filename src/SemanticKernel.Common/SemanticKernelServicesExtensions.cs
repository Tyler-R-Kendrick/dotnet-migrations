using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
namespace SKCLI;

public static class SemanticKernelServicesExtensions
{
    public static IServiceCollection AddSemanticKernel(
        this IServiceCollection services,
        Action<IKernelBuilder>? configure = null)
    => services
        .AddSingleton<IKernelFactory, KernelFactory>()
        .AddSingleton(x => x.GetRequiredService<IKernelFactory>()
            .BuildKernel(configure));
}