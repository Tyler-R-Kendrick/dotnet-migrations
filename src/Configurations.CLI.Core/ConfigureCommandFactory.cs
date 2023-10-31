using System.CommandLine;
using Microsoft.Extensions.Configuration;

[assembly:CLSCompliant(true)]
#pragma warning disable CA1822 // Mark members as static
namespace Configurations.CLI.Core;
public class ConfigureCommandFactory(IConfiguration configuration)
{
    public Command CreateCommand()
    {
        var command = new Command("config", "Configure the application");
        command.AddAlias("c");
        command.AddCommand(CreateGetCommand());
        command.AddCommand(CreateSetCommand());
        return command;
    }

    private Command CreateGetCommand()
    {
        var keyArgument = new Argument<string>("--key", "The key to read");
        var command = new Command("get", "Get the application configuration")
        {
            keyArgument
        };
        command.SetHandler((string key) =>
        {
            var value = configuration.GetValue<string>(key);
            Console.Out.WriteLine(value);
        }, keyArgument);
        return command;
    }

    private Command CreateSetCommand()
    {
        var keyArgument = new Argument<string>("--key", "The key to read");
        var valueArgument = new Argument<string>("--value", "The value to read");
        var command = new Command("set", "Set the application configuration")
        {
            keyArgument, valueArgument
        };
        command.SetHandler((string key, string value) =>
        {
            configuration[key] = value;
            Console.Out.WriteLine($"Set {key} to {value}.");
        }, keyArgument, valueArgument);
        return command;
    }
}
