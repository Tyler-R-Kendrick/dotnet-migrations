using Microsoft.Extensions.DependencyInjection;
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
    public static IServiceCollection AddSemanticCLI(this IServiceCollection services)
    => services
        .AddCLI()
        .AddSemanticKernel()
        //.AddTransient<IFunctionCommandFactory, FunctionCommandFactory>()
        .AddSingleton(Console.In)
        .AddSingleton(Console.Out)
        //.AddTransient<IFunctionCommandBuilder, FunctionCommandBuilder>()
        //.AddTransient<IPluginCommandBuilder, PluginCommandBuilder>()
        //.AddTransient<IPluginCommandFactory, PluginCommandFactory>()
        //.AddTransient<IFunctionsCommandFactory, FunctionsCommandFactory>()
        .AddTransient<IRootCommandBuilder, RootCommandBuilder>();
}