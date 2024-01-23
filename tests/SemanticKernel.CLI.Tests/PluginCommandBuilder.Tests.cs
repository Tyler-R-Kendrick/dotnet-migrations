using System.CommandLine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;
namespace SemanticKernel.CLI.Tests;

internal sealed class PluginCommandBuilderTests : TestBase<PluginCommandBuilder>
{
    private Mock<ICommandFactory> _mockCommandFactory = default!;
    private Mock<IFunctionsCommandFactory> _mockFunctionsCommandFactory = default!;
    private Mock<IFunctionCommandBuilder> _mockFunctionCommandBuilder = default!;

    override protected void SetupDependencies()
    {
        _mockCommandFactory = new Mock<ICommandFactory>();
        _mockFunctionsCommandFactory = new Mock<IFunctionsCommandFactory>();
        _mockFunctionCommandBuilder = new Mock<IFunctionCommandBuilder>();
    }

    override protected PluginCommandBuilder Allocate() => new PluginCommandBuilder(
        _mockCommandFactory.Object,
        _mockFunctionsCommandFactory.Object,
        _mockFunctionCommandBuilder.Object);

    [Test]
    public void BuildPluginCommand()
    {
        // Arrange
        const string
            commandInput = "command input",
            testParam = "test_param";
        var mockKernel = new Mock<IKernel>();
        var plugins = new[] { "plugin1", "plugin2" };
        var mockSkillCollection = new Mock<IReadOnlySkillCollection>();
        // kernel.Setup(x => x.GetRegisteredPlugins())
        //     .Returns(plugins);
        var functionsCommand = new Command("functions");
        var fsView = new FunctionsView();
        var fView = new FunctionView("test_skill", "test_function", "test description",
            new List<ParameterView> { new ("input", commandInput), new (testParam, default) }, false, false);
        _mockFunctionsCommandFactory.Setup(x => x.BuildFunctionsCommand(mockKernel.Object, plugins))
            .Returns(functionsCommand);
        var functionCommand = new Command("function");
        functionCommand.AddArgument(new Argument<string>("name"));
        _mockFunctionCommandBuilder.Setup(x => x.BuildFunctionCommand(mockKernel.Object, It.IsAny<Argument<string>>()))
            .Returns(functionCommand);
        mockSkillCollection.Setup(x => x.GetFunctionsView(true, true))
            .Returns(fsView);
        mockKernel.Setup(x => x.Skills).Returns(mockSkillCollection.Object);
        var expectedCommand = new Command("--plugin", "Select a plugin");
        expectedCommand.AddAlias("-p");
        expectedCommand.AddArgument(new Argument<string>("name")
            .FromAmong(plugins.ToArray()));
        expectedCommand.AddCommand(functionCommand);
        expectedCommand.AddCommand(functionsCommand);
        expectedCommand.SetHandler(_ => Task.CompletedTask);
        
        _mockCommandFactory
            .Setup(x => x.Create(
                "--plugin",
                It.IsAny<Func<Task>>(),
                It.IsAny<string[]?>(),
                "Select a plugin",
                It.IsAny<IEnumerable<Option>>(),
                It.IsAny<IEnumerable<Argument>>(),
                It.IsAny<IEnumerable<Command>>()))
            .Returns(expectedCommand);
        
        // Act
        var result = Concern.BuildPluginCommand(mockKernel.Object);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(expectedCommand));
        result.Invoke("--plugin plugin1");
        result.Invoke("--plugin plugin1 -f function");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("--plugin"));
        Assert.That(result.Description, Is.EqualTo("Select a plugin"));
        Assert.That(result.Aliases, Is.EquivalentTo(new[] { "--plugin", "-p" }));
        Assert.That(result.Arguments, Is.Not.Null);
        Assert.That(result.Arguments, Has.Count.EqualTo(1));
        Assert.That(result.Arguments[0].Name, Is.EqualTo("name"));
    }
}
