using System.CommandLine;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
using SKCLI;
[assembly: ComVisible(false)]

namespace SKCLI;

#pragma warning disable CS1591

public static class Program
{
    public static async Task Main(string[] args)
    {
        var kernelSettings = KernelSettings.LoadSettings();
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(kernelSettings.LogLevel ?? LogLevel.Warning)
                .AddConsole()
                .AddDebug();
        });
        var logger = loggerFactory.CreateLogger<IKernel>();
        var command = CommandFactory.BuildRootCommand(
            context => Console.WriteLine(context.Result),
            kernelSettings, logger);
        await command.InvokeAsync(args).ConfigureAwait(false);
    }
}