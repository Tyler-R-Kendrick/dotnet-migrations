using System.Runtime.CompilerServices;
using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SKCLI;
[assembly: InternalsVisibleTo("SemanticKernel.CLI.Tests"), InternalsVisibleTo("DynamicProxyGenAssembly2")]

var _loggerMessage =
    LoggerMessage.Define<string>(LogLevel.Information,
        new EventId(id: 0, name: "ERROR"), "{Message}");

var services = new ServiceCollection()
    .AddSemanticCLI()
    .AddLogging(x => x.AddConsole().AddDebug())
    .AddSingleton(x => KernelSettings.LoadSettings())
    .AddSingleton(x => new KernelFactory()
        .BuildKernel(x.GetRequiredService<KernelSettings>(),
            x.GetRequiredService<ILogger<IKernel>>()))
    .AddTransient(x => x.GetRequiredService<IRootCommandBuilder>()
        .BuildRootCommand(
            context => Console.Out.WriteLine(context.Result),
            x.GetRequiredService<IKernel>()));

var commandTask = services.BuildServiceProvider()
    .GetRequiredService<RootCommand>()
    .InvokeAsync(args)
    .ConfigureAwait(false);

await commandTask;
