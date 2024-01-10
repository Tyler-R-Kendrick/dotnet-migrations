using NUnit;
using Migrant.CLI;

namespace Migrant.CLI.Tests;

public abstract class TestBase<T>
{
    private readonly Lazy<T> _testConcern;
    protected T Concern => _testConcern.Value;

    [SetUp]
    public void Setup()
    {
        _testConcern = new Lazy<T>(Allocate);
    }

    protected abstract T Allocate();
}

public class Tests : TestBase<Command>
{
    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}