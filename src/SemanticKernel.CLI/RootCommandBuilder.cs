using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using InterfaceGenerator;

namespace SKCLI;

[InterfaceGenerator]
internal sealed partial class RootCommandBuilder(
    IRootCommandFactory _rootCommandFactory,
    IArgumentFactory _argumentFactory,
    TextWriter _writer)
{
    public RootCommand BuildRootCommand(
        Kernel kernel)
    {
        var inputArgument = _argumentFactory.Create<string>("input");
        var rootCommand = _rootCommandFactory.Create(
            "semker",
            "Execute plugins with the semantic kernal.",
            false,
            arguments: [inputArgument],
            options: [],
            subcommands: []);
        rootCommand.SetHandler(async input =>
        {
            _writer.WriteLine($"input: {input}");
            kernel.ImportPluginFromPromptDirectory("skills");
            var plannerOptions = new HandlebarsPlannerOptions() { AllowLoops = true };
            var planner = new HandlebarsPlanner(plannerOptions);
            var plan = await planner.CreatePlanAsync(kernel, input, CancellationToken.None);
            _writer.WriteLine($"plan: {plan}");
            var prompt = plan.Prompt;
            _writer.WriteLine($"prompt: {prompt}");
            var result = await plan.InvokeAsync(kernel);
            _writer.WriteLine($"result: {result}");
        }, inputArgument);
        return rootCommand;
    }

}
