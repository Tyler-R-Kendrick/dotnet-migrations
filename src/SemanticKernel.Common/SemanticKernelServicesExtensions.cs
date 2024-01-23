using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
namespace SKCLI;

public static class SemanticKernelServicesExtensions
{
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services)
    => services
        .AddSingleton(x => KernelSettings.LoadSettings())
        .AddSingleton<IKernelFactory, KernelFactory>()
        .AddSingleton(x => x.GetRequiredService<IKernelFactory>()
            .BuildKernel(x.GetRequiredService<KernelSettings>(),
                x.GetRequiredService<ILogger<IKernel>>()));

}