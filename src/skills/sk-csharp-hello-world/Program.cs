using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;

#pragma warning disable CA2007

var rootCommand = new System.CommandLine.RootCommand("Execute skills with the semantic kernal.");
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

var directoryOption = new Option<DirectoryInfo?>(
    name: "--directory",
    description: "The directory to read and load skills.");
var skillOption = new Option<string?>(
    name: "--skill",
    description: "The skill to execute.");
var outputOption = new Option<FileInfo?>(
    name: "--output",
    description: "The file to output on execution.");
rootCommand.AddOption(directoryOption);
rootCommand.AddOption(skillOption);
rootCommand.AddOption(outputOption);

rootCommand.SetHandler(async (directory, skillName, output) => {
    // note: using skills from the repo
    var currentDirectory = System.IO.Directory.GetCurrentDirectory();
    var skillsDirectory = Path.Combine(currentDirectory, "skills");
    var skill = kernel.ImportSemanticSkillFromDirectory(skillsDirectory,
        directory?.FullName ?? "FileConversion");
    #pragma warning disable CA1303
    Console.WriteLine("running skill");

    var context = new ContextVariables();
    var input = File.ReadAllText(Path.Combine(currentDirectory, "Program.cs"));
    context.Set("input", input);
    Console.WriteLine("set input");

    var task = (skillName is not null)
        ? kernel.RunAsync(context, skill[skillName ?? "Boyscouting"])
        : kernel.RunAsync(context, skill.Values.ToArray());
    var result = await task;
    Console.WriteLine(result);
    Console.WriteLine("finished results");
}, directoryOption, skillOption, outputOption);

await rootCommand.InvokeAsync(args);
