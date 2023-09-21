using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
namespace SKCLI;

#pragma warning disable CA2007
internal static class KernelFactory
{
    internal static IKernel BuildKernel(KernelSettings settings) 
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(settings.LogLevel ?? LogLevel.Warning)
                .AddConsole()
                .AddDebug();
        });

        IKernel kernel = new KernelBuilder()
            .WithLogger(loggerFactory.CreateLogger<IKernel>())
            .WithCompletionService(settings)
            .Build();
        return kernel;
    }
}
