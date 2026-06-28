using Application.Abstractions.Authentication;
using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTests.IntegrationTests;

public abstract class BaseTest : IClassFixture<UnitTestingApiFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly HomeAppContext DbContext;
    protected readonly IExecutionContextAccessor ExecutionContext;

    protected BaseTest(UnitTestingApiFactory factory, IExecutionContextAccessor executionContext = null)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider
            .GetRequiredService<HomeAppContext>();
        ExecutionContext = executionContext;
    }

    public void Dispose() => _scope.Dispose();
}
