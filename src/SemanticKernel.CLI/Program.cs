using System.CommandLine;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
[assembly: ComVisible(false)]
[assembly: InternalsVisibleTo("SemanticKernel.CLI.Tests")]
namespace SKCLI;

#pragma warning disable CS1591

public interface IProgram
{
    private static Action <ILogger, string, Exception?> _loggerMessage =
        LoggerMessage.Define<string>(LogLevel.Information,
            new EventId(id: 0, name: "ERROR"), "{Message}");
    Task MainAsync(params string[] args) => Main(args);
    static async Task Main(params string[] args)
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
            context => _loggerMessage(logger, context.Result.ToString(), null),
            kernel);
        await command.InvokeAsync(args).ConfigureAwait(false);
    }
}

public class Program : IProgram
{
}