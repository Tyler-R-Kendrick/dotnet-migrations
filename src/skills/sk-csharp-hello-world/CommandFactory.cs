using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
namespace SKCLI;

public static class CommandFactory
{
    public static RootCommand BuildRootCommand()
    {
        var rootCommand = new System.CommandLine.RootCommand("Execute skills with the semantic kernal.");
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
            var kernelSettings = KernelSettings.LoadSettings();
            var kernel = KernelFactory.BuildKernel(kernelSettings);
            var skills = kernel.ImportSemanticSkillFromDirectory(skillsDirectory,
                directory?.FullName ?? "FileConversion");
            #pragma warning disable CA1303
            Console.WriteLine("running skill");

            var context = new ContextVariables();
            var input = File.ReadAllText(Path.Combine(currentDirectory, "Program.cs"));
            context.Set("input", input);
            Console.WriteLine("set input");

            var task = (skillName is not null)
                ? kernel.RunAsync(context, skills[skillName ?? "Boyscouting"])
                : kernel.RunAsync(context, skills.Values.ToArray());
            var result = await task.ConfigureAwait(false);
            Console.WriteLine(result);
            Console.WriteLine("finished results");
        }, directoryOption, skillOption, outputOption);
        return rootCommand;
    }

}