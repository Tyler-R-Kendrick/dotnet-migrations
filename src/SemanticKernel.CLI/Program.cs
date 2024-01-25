using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using SKCLI;

var services = new ServiceCollection()
    .AddSemanticCLI();

var commandTask = services.BuildServiceProvider()
    .GetRequiredService<RootCommand>()
    .InvokeAsync(args)
    .ConfigureAwait(false);

await commandTask;
