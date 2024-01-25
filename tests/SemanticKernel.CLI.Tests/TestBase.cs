using Microsoft.Extensions.DependencyInjection;

namespace SemanticKernel.CLI.Tests;

public abstract class TestBase<T>
    where T : notnull
{
    private Lazy<T> _testConcern;
    protected T Concern => _testConcern.Value;
    private IServiceCollection _services = default!;

    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();
        SetupDependencies(_services);
        _testConcern = new Lazy<T>(() => Allocate(_services.BuildServiceProvider()));        
    }

    protected virtual T Allocate(IServiceProvider provider)
        => provider.GetRequiredService<T>();

    protected virtual void SetupDependencies(IServiceCollection services)
    {
    }
}
