using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System.Linq;
using System.Diagnostics.Contracts;
namespace SKCLI;
#pragma warning disable CS3001
public static class CommandFactory
{
    public static void BuildSkillCommands(
        IKernel kernel,
        RootCommand rootCommand,
        DirectoryInfo directoryInfo) 
    {
        Contract.Assert(directoryInfo != null);
        foreach(var directory in directoryInfo.GetDirectories().Distinct())
        {
            Console.WriteLine(directory);
            var skills = kernel.ImportSemanticSkillFromDirectory(
                directoryInfo.Name,
                directory.Name);
            foreach(var (key, skill) in skills)
            {
                Contract.Assert(rootCommand != null);
                if(rootCommand.Children.Any(x => x.Name == skill.SkillName))
                    continue;
                var command = new Command(skill.SkillName, skill.Description);
                var parameters = skill.Describe().Parameters;
                foreach(var @param in parameters)
                {
                    var option = new Option<string?>(@param.Name,
                        () => @param.DefaultValue,
                        @param.Description);
                    command.AddOption(option);
                }
                command.SetHandler(async () =>
                    await skill.InvokeAsync().ConfigureAwait(false));
                rootCommand.Add(command);
            }
        }
    }
    public static RootCommand BuildRootCommand()
    {
        var rootCommand = new System.CommandLine.RootCommand("Execute skills with the semantic kernal.");
        rootCommand.Name = "semker";
        var skillOption = new Option<string?>(
            name: "--skill",
            description: "The skill to execute.")
            {
                IsRequired = true
            };
        rootCommand.AddOption(skillOption);

        var currentDirectory = System.IO.Directory.GetCurrentDirectory();
        var skillsDirectory = Path.Combine(currentDirectory, "skills");
        Contract.Assert(Directory.Exists(skillsDirectory));
        var kernelSettings = KernelSettings.LoadSettings();
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(kernelSettings.LogLevel ?? LogLevel.Warning)
                .AddConsole()
                .AddDebug();
        });
        var logger = loggerFactory.CreateLogger<IKernel>();
        var kernel = KernelFactory.BuildKernel(kernelSettings, logger);
        foreach(var subdir in new DirectoryInfo(skillsDirectory).GetDirectories())
        {
            BuildSkillCommands(kernel, rootCommand, subdir);
        }
        return rootCommand;
    }

}