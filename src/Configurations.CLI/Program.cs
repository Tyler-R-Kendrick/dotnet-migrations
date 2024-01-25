using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Configurations.CLI.Core;
using System.CommandLine;
using System.CommandLine.Hosting;

[assembly:CLSCompliant(true)]
#pragma warning disable CA1303 // Do not pass literals as localized parameters

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddInMemoryCollection(
    new Dictionary<string, string?>
    {
        ["SecretKey"] = "Dictionary MyKey Value",
        ["TransientFaultHandlingOptions:Enabled"] = bool.TrueString,
        ["TransientFaultHandlingOptions:AutoRetryDelay"] = "00:00:07",
        ["Logging:LogLevel:Default"] = "Warning"
    });

using IHost host = builder.Build();

var cliConfig = await BuildCommandLine(host)
    .InvokeAsync(args)
    .ConfigureAwait(false);


static RootCommand BuildCommandLine(IHost host)
{
    var rootCommand = new RootCommand(@"$ dotnet run ");
    IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();
    var command = new ConfigureCommandFactory(configuration).CreateCommand();
    rootCommand.AddCommand(command);
    return rootCommand;
}
