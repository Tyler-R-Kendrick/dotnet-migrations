using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SKCLI;

var services = new ServiceCollection()
    .AddSemanticCLI()
    .AddLogging(x => x.AddConsole().AddDebug())
    .AddTransient(x => x.GetRequiredService<IRootCommandBuilder>()
        .BuildRootCommand(x.GetRequiredService<Kernel>()));

var commandTask = services.BuildServiceProvider()
    .GetRequiredService<RootCommand>()
    .InvokeAsync(args)
    .ConfigureAwait(false);

await commandTask;
