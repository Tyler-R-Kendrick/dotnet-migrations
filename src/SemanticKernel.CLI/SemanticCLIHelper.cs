using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
namespace SKCLI;

/// <summary>
/// Helper class for configuring Semantic CLI services.
/// </summary>
public static class SemanticCLIHelper
{
    /// <summary>
    /// Adds Semantic CLI services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSemanticCLI(
        this IServiceCollection services,
        Action<ILoggingBuilder>? configureLogging = null,
        Action<IKernelBuilder>? configureKernel = null)
    => services
        .AddCLI()
        .AddSemanticKernel(configureLogging, configureKernel)
        .AddSingleton(Console.In)
        .AddSingleton(Console.Out)
        .AddTransient<IRootCommandBuilder, RootCommandBuilder>()
        .AddTransient(x => x.GetRequiredService<IRootCommandBuilder>()
            .BuildRootCommand(x.GetRequiredService<Kernel>()));
}