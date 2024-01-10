namespace SemanticKernel.CLI.Tests;

public abstract class TestBase<T>
{
    private Lazy<T> _testConcern;
    protected T Concern => _testConcern.Value;

    [SetUp]
    public void Setup()
    {
        SetupDependencies();
        _testConcern = new Lazy<T>(Allocate);
    }

    protected abstract T Allocate();

    protected virtual void SetupDependencies()
    {
    }
}
