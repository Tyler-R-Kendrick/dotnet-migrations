using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace SKCLI;

/// <summary>
/// Represents the program interface.
/// </summary>
public interface IProgram
{
    private static readonly Action <ILogger, string, Exception?> _loggerMessage =
        LoggerMessage.Define<string>(LogLevel.Information,
            new EventId(id: 0, name: "ERROR"), "{Message}");

    /// <summary>
    /// Represents an asynchronous operation that returns a task.
    /// </summary>
    /// <param name="args">The arguments passed to the MainAsync method.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task MainAsync(params string[] args) => Main(args);

    /// <summary>
    /// Represents an asynchronous operation that can return a result.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    static async Task Main(params string[] args)
    {
        var kernelSettings = KernelSettings.LoadSettings();
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder
            .SetMinimumLevel(kernelSettings.LogLevel ?? LogLevel.Warning)
            .AddConsole()
            .AddDebug());
        var logger = loggerFactory.CreateLogger<IKernel>();
        var kernelFactory = new KernelFactory();
        var kernel = kernelFactory.BuildKernel(kernelSettings, logger);
        var commandFactory = new CommandFactory();
        var optionsFactory = new OptionFactory();
        var argumentFactory = new ArgumentFactory();
        var @in = Console.In;
        var @out = Console.Out;
        var functionCommandFactory = new FunctionCommandFactory(
            commandFactory,
            optionsFactory);
        var functionCommandBuilder = new FunctionCommandBuilder(
            argumentFactory,
            commandFactory,
            @out,
            @in);
        var functionsCommandFactory = new FunctionsCommandFactory(
            @out);
        var pluginCommandBuiler = new PluginCommandBuilder(
            commandFactory,
            functionsCommandFactory,
            functionCommandBuilder);
        var pluginCommandFactory = new PluginCommandFactory(
            functionCommandFactory);
        var rootCommandFactory = new RootCommandFactory();
        var rootCommandBuilder = new RootCommandBuilder(
            rootCommandFactory,
            pluginCommandFactory,
            pluginCommandBuiler,
            optionsFactory,
            argumentFactory,
            Console.Out);
        var command = rootCommandBuilder.BuildRootCommand(
            context => Console.Out.WriteLine(context.Result),
            kernel);
        await command.InvokeAsync(args).ConfigureAwait(false);
    }
}