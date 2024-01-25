using System.Reflection;

namespace SemanticKernel.CLI.Tests;

public class ProgramUnitTests
{
    [TestCase(@"semker ""Come up with something fun.""")]
    public void MainExecutesSuccessfully(string args)
    {
        var assembly = Assembly.LoadFrom("SemanticKernel.CLI.dll");
        var entryPoint = assembly.EntryPoint;

        Assert.That(entryPoint, Is.Not.Null);
        entryPoint.Invoke(null, new object[] { new string[] { args } });
    }
}