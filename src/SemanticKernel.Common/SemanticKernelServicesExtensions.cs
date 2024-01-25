using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
namespace SKCLI;

public static class SemanticKernelServicesExtensions
{
    public static IServiceCollection AddSemanticKernel(
        this IServiceCollection services,
        Action<ILoggingBuilder>? configureLogging = null,
        Action<IKernelBuilder>? configure = null)
    => services
        .AddSingleton<IKernelFactory, KernelFactory>()
        .AddLogging(configureLogging ?? (x => x.AddConsole()))
        .AddSingleton(x => x.GetRequiredService<IKernelFactory>()
            .BuildKernel(
                x.GetRequiredService<ILogger<KernelFactory>>(),
                configure));
}