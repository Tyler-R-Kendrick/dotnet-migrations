using System.CommandLine;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SKCLI;

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
    //[TestCase("-p FunSkill -?")]
    // [TestCase("-p FunSkill --functions")]
    // [TestCase("-p FunSkill -f Joke")]
    // [TestCase("-p FunSkill -f Joke -?")]
    // [TestCase(@"-p FunSkill -f Joke --input ""Some Text""")]
    public async Task MainExecutesSuccessfully(string args)
    {
        await IProgram.Main(args);
    }
}