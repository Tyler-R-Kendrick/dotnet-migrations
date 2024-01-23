using System.Reflection;

namespace SemanticKernel.CLI.Tests;

public class ProgramUnitTests
{
    [TestCase("--plugins")]
    [TestCase("-P")]
    [TestCase("--version")]
    [TestCase("--help")]
    [TestCase("-h")]
    [TestCase("-?")]
    [TestCase("FunSkill -?")]
    [TestCase("FunSkill -h")]
    [TestCase("FunSkill --help")]
    [TestCase("FunSkill --functions")]
    [TestCase("FunSkill Joke -?")]
    [TestCase(@"FunSkill ""Tell me something fun.""")]
    //[TestCase(@"-p ""FunSkill"" --functions")]
    // [TestCase("-p FunSkill -f Joke")]
    // [TestCase("-p FunSkill -f Joke -?")]
    // [TestCase(@"-p FunSkill -f Joke --input ""Some Text""")]
    public void MainExecutesSuccessfully(string args)
    {
        //TODO: Get the entry point of the assembly "SemanticKernel.CLI" and call it with the args.
        var assembly = Assembly.LoadFrom("SemanticKernel.CLI.dll");
        var entryPoint = assembly.EntryPoint;

        Assert.That(entryPoint, Is.Not.Null);
        entryPoint.Invoke(null, new object[] { new string[] { args } });
    }
}