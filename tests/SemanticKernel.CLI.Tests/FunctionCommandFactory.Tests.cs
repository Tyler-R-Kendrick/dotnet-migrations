using System.CommandLine;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
namespace SemanticKernel.CLI.Tests;

public class FunctionCommandFactoryTests : TestBase<IFunctionCommandFactory>
{
    private Mock<ICommandFactory> _mockCommandFactory = default!;
    private Mock<IOptionFactory> _mockOptionFactory = default!;
    override protected IFunctionCommandFactory Allocate()
    => new FunctionCommandFactory(
        _mockCommandFactory.Object,
        _mockOptionFactory.Object);

    override protected void SetupDependencies()
    {
        _mockCommandFactory = new Mock<ICommandFactory>();
        _mockOptionFactory = new Mock<IOptionFactory>();
    }

    [Test]
    public void CreateSucceeds()
    {
        // Arrange
        var mockFunction = new Mock<ISKFunction>();
        var mockDelegate = new Mock<Action<SKContext>>();
        var expectedCommand = new Command("name", "description");
        mockFunction.Setup(x => x.Name).Returns("name");
        mockFunction.Setup(x => x.Description).Returns("description");
        mockFunction.Setup(x => x.Describe()).Returns(new FunctionView());
        _mockCommandFactory.Setup(x => 
            x.Create(
                mockFunction.Object.Name,
                It.IsAny<Func<Task>>(),
                It.IsAny<string[]>(),
                mockFunction.Object.Description,
                It.IsAny<IEnumerable<Option>>(),
                It.IsAny<IEnumerable<Argument>>(),
                It.IsAny<IEnumerable<Command>>()))
            .Returns(expectedCommand);

        // Act
        var actualCommand = Concern.Create(mockFunction.Object, mockDelegate.Object);

        // Assert
        Assert.That(actualCommand, Is.EqualTo(expectedCommand));
    }
}