using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;

#pragma warning disable CA2007

var kernelSettings = KernelSettings.LoadSettings();

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(kernelSettings.LogLevel ?? LogLevel.Warning)
        .AddConsole()
        .AddDebug();
});

IKernel kernel = new KernelBuilder()
    .WithLogger(loggerFactory.CreateLogger<IKernel>())
    .WithCompletionService(kernelSettings)
    .Build();

if (kernelSettings.EndpointType == EndpointTypes.TextCompletion)
{
    // note: using skills from the repo
    var currentDirectory = System.IO.Directory.GetCurrentDirectory();
    var skillsDirectory = Path.Combine(currentDirectory, "skills");
    var skill = kernel.ImportSemanticSkillFromDirectory(skillsDirectory, "FileConversion");
    #pragma warning disable CA1303
    Console.WriteLine("running skill");

    var context = new ContextVariables();
    var input = File.ReadAllText(Path.Combine(currentDirectory, "Program.cs"));
    context.Set("input", input);
    Console.WriteLine("set input");

    var result = await kernel.RunAsync(context, skill["Boyscouting"]);
    Console.WriteLine(result);
    Console.WriteLine("finished results");
}
