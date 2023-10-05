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
        var command = new Command("get", "Get the application configuration");
        var keyOption = new Option<string>(new string[] { "--key", "-k" }, "The key to read");
        keyOption.IsRequired = true;
        command.AddOption(keyOption);
        command.SetHandler((string key) =>
        {
            var value = configuration.GetValue<string>(key);
            Console.Out.WriteLine(value);
        }, keyOption);
        return command;
    }

    private Command CreateSetCommand()
    {
        var command = new Command("set", "Set the application configuration");
        var keyOption = new Option<string>(new string[] { "--key", "-k" }, "The key to read");
        keyOption.IsRequired = true;
        var valueOption = new Option<string>(new string[] { "--value", "-v" }, "The value to read");
        command.AddOption(keyOption);
        command.AddOption(valueOption);
        command.SetHandler((string key, string value) =>
        {
            configuration[key] = value;
            Console.Out.WriteLine($"Set {key} to {value}.");
        }, keyOption, valueOption);
        return command;
    }
}
