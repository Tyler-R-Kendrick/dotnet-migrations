using NUnit.Framework;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.CommandLine;
using Microsoft.SemanticKernel.SkillDefinition;
using SKCLI;

namespace SemanticKernel.CLI.Tests;

public class OptionFactoryTests : TestBase<IOptionFactory>
{
    override protected IOptionFactory Allocate()
        => new OptionFactory();

    [Test]
    public void TestCreateOptions()
    {
        // Arrange
        var mockFunction = new Mock<ISKFunction>();
        var parameterView = new ParameterView
        {
            Name = "test", 
            DefaultValue = "default", 
            Description = "description"
        };
        var functionView = new FunctionView
        {
            Parameters = new List<ParameterView> { parameterView }
        };
        mockFunction
            .Setup(f => f.Describe())
            .Returns(functionView);

        // Act
        var options = Concern.CreateOptions(mockFunction.Object);

        // Assert
        var option = options.First();
        Assert.That(option.Aliases.First(), Is.EqualTo("--test"));
        Assert.That(option.Description, Is.EqualTo("description"));
    }

    [Test]
    public void TestCreateOption()
    {
        // Arrange
        var parameterView = new ParameterView
        {
            Name = "test", 
            DefaultValue = "default", 
            Description = "description"
        };

        // Act
        var option = Concern.CreateOption(parameterView);

        // Assert
        Assert.That(option.Aliases.First(), Is.EqualTo("--test"));
        Assert.That(option.Description, Is.EqualTo("description"));
    }
}