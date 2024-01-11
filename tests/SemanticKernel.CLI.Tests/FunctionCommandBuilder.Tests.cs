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
    override protected FunctionCommandBuilder Allocate()
    => new FunctionCommandBuilder(
        _mockTextWriter.Object,
        _mockTextReader.Object);

    override protected void SetupDependencies()
    {
        _mockTextWriter = new Mock<TextWriter>();
        _mockTextReader = new Mock<TextReader>();
    }

    [Test]
    public async Task BuildFunctionCommandSucceeds()
    {
        const string
            pluginName = "plugin",
            functionName = "function",
            commandInput = "command input",
            testParam = "test_param",
            expectedResult = "test result";
        // Arrange
        var argument = new Argument<string>("plugin");
        var mockKernel = new Mock<IKernel>();
        var mockLogger = new Mock<ILogger>();
        var mockSkillCollection = new Mock<IReadOnlySkillCollection>();
        var mockSKFunction = new Mock<ISKFunction>();
        
        var fsView = new FunctionsView();
        var fView = new FunctionView("test_skill", "test_function", "test description",
            new List<ParameterView> { new ("input", commandInput), new (testParam, default) }, false, false);
        fsView.AddFunction(fView);
        mockSKFunction.Setup(x => x.Describe())
            .Returns(fView);
        mockSKFunction.Setup(x => x.InvokeAsync(It.IsAny<SKContext>(), null, default))
            .ReturnsAsync(new SKContext(new(expectedResult), mockSkillCollection.Object, mockLogger.Object));
        mockSkillCollection.Setup(x => x.GetFunctionsView(true, true))
            .Returns(fsView);
        mockKernel.Setup(x => x.Logger).Returns(mockLogger.Object);
        mockKernel.Setup(x => x.Skills).Returns(mockSkillCollection.Object);
        mockKernel.Setup(x => x.Func(pluginName, functionName))
            .Returns(mockSKFunction.Object);

        // Act
        var actualCommand = Concern.BuildFunctionCommand(
            mockKernel.Object, argument);
        await actualCommand.InvokeAsync($"{pluginName} {functionName} {commandInput}");

        // Assert
        Assert.That(actualCommand, Is.InstanceOf<Command>());
        Assert.That(actualCommand.Name, Is.EqualTo("--function"));
        Assert.That(actualCommand.Description, Is.EqualTo("The function to execute"));
        Assert.That(actualCommand.TreatUnmatchedTokensAsErrors, Is.False);
        Assert.That(actualCommand.Aliases, Is.EquivalentTo(new[] { "--function", "-f" }));
        Assert.That(actualCommand.Arguments, Contains.Item(argument));
        Assert.That(actualCommand.Handler, Is.Not.Null);

        _mockTextWriter.Verify(x => x.WriteLineAsync(It.IsAny<string>()), Times.Exactly(2));
        _mockTextWriter.Verify(x => x.WriteLineAsync(expectedResult), Times.Exactly(1));
        _mockTextWriter.Verify(x => x.WriteAsync($"{testParam}: "), Times.Exactly(1));
        _mockTextReader.Verify(x => x.ReadLineAsync(), Times.Exactly(1));
    }
}