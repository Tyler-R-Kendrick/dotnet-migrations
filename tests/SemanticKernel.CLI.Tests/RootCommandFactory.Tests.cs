using SKCLI;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using Microsoft.SemanticKernel.Orchestration;
using System.Diagnostics.Contracts;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SemanticKernel.CLI.Tests;

public class RootCommandBuilderUnitTests : TestBase<IRootCommandBuilder>
{
    private Mock<IRootCommandFactory> _mockRootCommandFactory = default!;
    private Mock<IPluginCommandFactory> _mockPluginCommandFactory = default!;
    private Mock<TextWriter> _mockTextWriter = default!;
    private Mock<IPluginCommandBuilder> _mockPluginBuilder = default!;
    private Mock<IOptionFactory> _mockOptionFactory = default!;
    private Mock<IArgumentFactory> _mockArgumentFactory = default!;

    override protected IRootCommandBuilder Allocate()
    => new RootCommandBuilder(
        _mockRootCommandFactory.Object,
        _mockPluginCommandFactory.Object,
        _mockPluginBuilder.Object,
        _mockOptionFactory.Object,
        _mockArgumentFactory.Object,
        _mockTextWriter.Object);

    override protected void SetupDependencies()
    {
        _mockRootCommandFactory = new Mock<IRootCommandFactory>();
        _mockPluginCommandFactory = new Mock<IPluginCommandFactory>();
        _mockTextWriter = new Mock<TextWriter>();
        _mockPluginBuilder = new Mock<IPluginCommandBuilder>();
        _mockOptionFactory = new Mock<IOptionFactory>();
        _mockArgumentFactory = new Mock<IArgumentFactory>();
    }

    [Test]
    public void BuildRootCommandSucceeds()
    {
        // Arrange
        var mockKernel = new Mock<IKernel>();
        var mockLogger = new Mock<ILogger>();
        var mockSkillCollection = new Mock<IReadOnlySkillCollection>();
        var expectedRootCommand = new RootCommand("semker");
        mockSkillCollection.Setup(x => x.GetFunctionsView(true, true)).Returns(new FunctionsView());
        mockKernel.Setup(x => x.Logger).Returns(mockLogger.Object);
        mockKernel.Setup(x => x.Skills).Returns(mockSkillCollection.Object);
        
        var command = new Command("name", "description");
        _mockRootCommandFactory
            .Setup(x => x.Create(
                "semker",
                "Execute plugins with the semantic kernal.",
                false,
                It.IsAny<Argument[]>(),
                It.IsAny<Option[]>(),
                It.IsAny<Command[]>()))
            .Returns(expectedRootCommand);
        _mockPluginBuilder
            .Setup(x => x.BuildPluginCommand(mockKernel.Object))
            .Returns(command);
        _mockPluginCommandFactory
            .Setup(x => x.GetPluginsDirectory())
            .Returns(new DirectoryInfo(Directory.GetCurrentDirectory()));
        _mockPluginCommandFactory
            .Setup(x => x.CreateCommands(
                mockKernel.Object,
                new DirectoryInfo(Directory.GetCurrentDirectory()),
                It.IsAny<Action<SKContext>>()))
            .Returns([command]);
        _mockOptionFactory
            .Setup(x => x.CreateOption<bool?>(
                It.IsAny<string[]>(),
                It.IsAny<ArgumentArity?>()))
            .Returns(new Option<bool?>("--plugins", "List all plugins."));

        // Act
        var actualCommand = Concern.BuildRootCommand(
            context => { },
            mockKernel.Object);

        // Assert
        Assert.That(actualCommand, Is.InstanceOf<RootCommand>());
        //Assert.That(actualCommand.Name, Is.EqualTo("semker"));

        var options = actualCommand.Options;
        Assert.That(options, Has.Count.EqualTo(1));
        Assert.That(options[0].Name, Is.EqualTo("plugins"));

        var handler = actualCommand.Handler;
        Assert.That(handler, Is.Not.Null);

        var arguments = actualCommand.Arguments;
        Assert.That(arguments, Has.Count.EqualTo(0));

        var aliases = actualCommand.Aliases;
        //Assert.That(aliases, Has.Count.EqualTo(2), $"Alias mismatch; found: {string.Join(", ", aliases)}.");

        var subcommands = actualCommand.Subcommands;
        Assert.That(subcommands, Has.Count.EqualTo(1));
        Assert.That(subcommands[0], Is.EqualTo(command));
    }
}