using Microsoft.Extensions.DependencyInjection;

namespace SKCLI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCLI(this IServiceCollection services)
    {
        services.AddSingleton<ICommandFactory, CommandFactory>();
        services.AddSingleton<IOptionFactory, OptionFactory>();
        services.AddSingleton<IArgumentFactory, ArgumentFactory>();
        services.AddSingleton<IRootCommandFactory, RootCommandFactory>();
        return services;
    }
}