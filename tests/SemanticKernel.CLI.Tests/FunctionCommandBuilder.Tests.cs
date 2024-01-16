using NUnit.Framework;
using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
namespace SemanticKernel.CLI.Tests;

internal sealed class FunctionCommandBuilderUnitTests : TestBase<FunctionCommandBuilder>
{
    private Mock<TextWriter> _mockTextWriter = default!;
    private Mock<TextReader> _mockTextReader = default!;
    private Mock<IArgumentFactory> _mockArgumentFactory = default!;
    private Mock<ICommandFactory> _mockCommandFactory = default!;

    override protected FunctionCommandBuilder Allocate()
    => new FunctionCommandBuilder(
        _mockArgumentFactory.Object,
        _mockCommandFactory.Object,
        _mockTextWriter.Object,
        _mockTextReader.Object);

    override protected void SetupDependencies()
    {
        _mockArgumentFactory = new Mock<IArgumentFactory>();
        _mockCommandFactory = new Mock<ICommandFactory>();
        _mockTextWriter = new Mock<TextWriter>();
        _mockTextReader = new Mock<TextReader>();
    }

    [Test]
    public async Task BuildFunctionCommandSucceeds()
    {
        const string
            pluginName = "test_skill",
            functionName = "test_function",
            commandInput = @"""command input""",
            testParam = "test_param",
            expectedResult = "test result";
        // Arrange
        var argument = new Argument<string>("plugin");
        var mockKernel = new Mock<IKernel>();
        var mockLogger = new Mock<ILogger>();
        var mockSkillCollection = new Mock<IReadOnlySkillCollection>();
        var mockSKFunction = new Mock<ISKFunction>();
        
        var fsView = new FunctionsView();
        var fView = new FunctionView("test_skill", functionName, "test description",
            new List<ParameterView> { new ("input", commandInput), new (testParam, default) }, false, false);
        fsView.AddFunction(fView);
        var nameArgument = new Argument<string>("name", "The name of the function to execute");
        _mockArgumentFactory
            .Setup(x => x.Create<string>(
                "name",
                "The name of the function to execute",
                It.IsAny<string?>(),
                It.IsAny<ArgumentArity?>(),
                It.IsAny<bool>()))
            .Returns(nameArgument);
        var expectedCommand = new Command("--function", "The function to execute");
        expectedCommand.AddAlias("-f");
        expectedCommand.AddArgument(argument);
        expectedCommand.AddArgument(nameArgument);
        expectedCommand.TreatUnmatchedTokensAsErrors = false;
        _mockCommandFactory
            .Setup(x => x.Create(
                "--function",
                "The function to execute"))
            .Returns(expectedCommand);
        mockSKFunction.Setup(x => x.Describe())
            .Returns(fView);
        mockSKFunction.Setup(x => x.InvokeAsync(It.IsAny<SKContext>(), null, default))
            .ReturnsAsync(new SKContext(new(expectedResult), mockSkillCollection.Object, mockLogger.Object));
        mockSkillCollection.Setup(x => x.GetFunctionsView(true, true))
            .Returns(fsView);
        mockKernel.Setup(x => x.Skills).Returns(mockSkillCollection.Object);
        mockKernel.Setup(x => x.Logger).Returns(mockLogger.Object);
        mockKernel.Setup(x => x.Func(pluginName, functionName))
            .Returns(mockSKFunction.Object);

        // Act
        var actualCommand = Concern.BuildFunctionCommand(
            mockKernel.Object, argument);
        await actualCommand.InvokeAsync($"{pluginName} {functionName}");

        // Assert
        Assert.That(actualCommand, Is.InstanceOf<Command>());
        Assert.That(actualCommand.Name, Is.EqualTo("--function"));
        Assert.That(actualCommand.Description, Is.EqualTo("The function to execute"));
        Assert.That(actualCommand.Aliases, Is.EquivalentTo(new[] { "--function", "-f" }));
        Assert.That(actualCommand.Arguments, Contains.Item(argument));
        Assert.That(actualCommand.Handler, Is.Not.Null);

        mockKernel.Verify(x => x.Func(pluginName, functionName), Times.Exactly(1));
        _mockTextWriter.Verify(x => x.WriteLineAsync(It.IsAny<string>()), Times.Exactly(2));
        _mockTextWriter.Verify(x => x.WriteLineAsync(expectedResult), Times.Exactly(1));
        _mockTextWriter.Verify(x => x.WriteAsync($"{testParam}: "), Times.Exactly(1));
        _mockTextReader.Verify(x => x.ReadLineAsync(), Times.Exactly(1));
    }
}