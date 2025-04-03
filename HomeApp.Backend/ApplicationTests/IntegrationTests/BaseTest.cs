using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTests.IntegrationTests;

public abstract class BaseTest : IClassFixture<UnitTestingApiFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly HomeAppContext DbContext;

    protected BaseTest(UnitTestingApiFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider
            .GetRequiredService<HomeAppContext>();
    }

    public void Dispose() => _scope.Dispose();
}
