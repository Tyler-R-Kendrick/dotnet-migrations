using System.CommandLine;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
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
        var kernel = KernelFactory.BuildKernel(kernelSettings, logger);
        var command = RootCommandFactory.BuildRootCommand(
            context => Console.WriteLine(context.Result),
            kernel);
        await command.InvokeAsync(args).ConfigureAwait(false);
    }
}