using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using System.Diagnostics.Contracts;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;
static class RootCommandFactory
{
    internal static RootCommand BuildRootCommand(
        Action<SKContext> onExecute,
        IKernel kernel)
    {
        var rootCommand = new RootCommand("Execute plugins with the semantic kernal.")
        {
            Name = "semker",
            TreatUnmatchedTokensAsErrors = false
        };
        BuildRootSubcommands(onExecute, kernel, rootCommand);
        BuildPluginsOption(kernel, rootCommand);
        BuildPluginCommand(kernel, rootCommand);
        return rootCommand;
    }

    private static void BuildPluginCommand(IKernel kernel, RootCommand rootCommand)
    {
        var plugins = GetRegisteredPlugins(kernel);
        var pluginCommand = new Command("--plugin", "Select a plugin");
        pluginCommand.AddAlias("-p");
        var pluginArgument = new Argument<string>("name")
            .FromAmong(plugins.ToArray());
        pluginCommand.AddArgument(pluginArgument);

        var functionsCommand = new Command("--functions", "list all functions for a plugin.");
        pluginCommand.AddCommand(functionsCommand);

        var functionCommand = new Command("--function", "The function to execute")
        {
            TreatUnmatchedTokensAsErrors = false
        };
        functionCommand.AddAlias("-f");
        var functionArgument = new Argument<string>("name");
        //todo: add argument validator.
        functionCommand.AddArgument(functionArgument);
        //todo: add handler for dynamic function args
        functionCommand.SetHandler(async (plugin, function) =>
        {
            var skFunction = kernel.Func(plugin, function);
            var view = skFunction.Describe();
            var parameters = view.Parameters;
            var contextVariables = new ContextVariables();
            foreach(var parameter in parameters)
            {
                Console.WriteLine(parameter.Description);
                Console.Write(parameter.Name + ": ");
                var value = Console.ReadLine();
                contextVariables[parameter.Name] = value ?? string.Empty;
            }
            var result = await skFunction.InvokeAsync(contextVariables).ConfigureAwait(false);
            Console.WriteLine(result.Result);
        }, pluginArgument, functionArgument);
        pluginCommand.AddCommand(functionCommand);



        //semker --plugins
        //semker -p FunSkill -?
        //semker -p FunSkill --functions
        //semker -p FunSkill -f Joke -?
        //semker -p FunSkill -f Joke
        //semker -p FunSkill -f Joke --input "Some Text"
        rootCommand.Add(pluginCommand);
    }

    private static void BuildPluginsOption(IKernel kernel, RootCommand rootCommand)
    {
        var writer = Console.Out;
        string[] aliases = ["--plugins", "-P"];
        var option = new Option<bool?>(aliases)
        {
            Arity = ArgumentArity.Zero
        };
        rootCommand.AddOption(option);
        rootCommand.SetHandler(async (options) =>
        {
            #pragma warning disable CS8602
            foreach (var key in GetRegisteredPlugins(kernel))
            {
                await writer.WriteLineAsync(key).ConfigureAwait(false);
            }
        }, option);
    }

    private static IDictionary<string, IEnumerable<FunctionView>> GetFunctionViews(IKernel kernel)
    {
        var skills = kernel.Skills.GetFunctionsView();
        var semantic = skills.SemanticFunctions;
        var native = skills.NativeFunctions;
        return semantic.Concat(native)
            .ToDictionary(x => x.Key, x => x.Value.AsEnumerable());
    }

    private static IEnumerable<string> GetRegisteredFunctions(IKernel kernel)
        => GetFunctionViews(kernel).SelectMany(x => x.Value.Select(y => y.Name));

    private static IEnumerable<string> GetRegisteredFunctions(IKernel kernel, string plugin)
        => GetFunctionViews(kernel)[plugin].Select(x => x.Name);

    private static IEnumerable<string> GetRegisteredPlugins(IKernel kernel)
        => GetFunctionViews(kernel).Keys;

    private static void BuildRootSubcommands(
        Action<SKContext> onExecute,
        IKernel kernel,
        RootCommand rootCommand,
        DirectoryInfo? directoryInfo = null)
    {
        var skillDirInfo = directoryInfo ?? PluginCommandFactory.GetPluginsDirectory();
        Contract.Assert(skillDirInfo.Exists);
        var subCommands = PluginCommandFactory.CreateCommands(kernel, skillDirInfo, onExecute);
        foreach (var subCommand in subCommands)
        {
            rootCommand.Add(subCommand);
        }
    }
}
