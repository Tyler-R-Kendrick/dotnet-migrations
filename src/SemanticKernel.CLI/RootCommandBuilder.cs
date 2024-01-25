using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using InterfaceGenerator;
using Microsoft.Extensions.Logging;

namespace SKCLI;

[InterfaceGenerator]
internal sealed partial class RootCommandBuilder(
    IRootCommandFactory _rootCommandFactory,
    IArgumentFactory _argumentFactory,
    TextWriter _writer,
    ILogger<RootCommandBuilder> _logger)
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
            _logger.LogTrace($"input: {input}");
            kernel.ImportPluginFromPromptDirectory("skills");
            var plannerOptions = new HandlebarsPlannerOptions() { AllowLoops = true };
            var planner = new HandlebarsPlanner(plannerOptions);
            var plan = await planner.CreatePlanAsync(kernel, input, CancellationToken.None);
            _logger.LogInformation($"input: {plan}");
            var prompt = plan.Prompt;
            _logger.LogTrace($"prompt: {prompt}");
            var result = await plan.InvokeAsync(kernel);
            result = string.IsNullOrEmpty(result) ? "No results." : result;
            _logger.LogTrace($"result: {result}");
            _writer.WriteLine(result);
        }, inputArgument);
        return rootCommand;
    }

}
