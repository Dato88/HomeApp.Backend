using Application.Abstractions.Authentication;
using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTests.IntegrationTests;

public abstract class BaseTest : IClassFixture<UnitTestingApiFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly HomeAppContext DbContext;
    protected readonly IUserContext UserContext;

    protected BaseTest(UnitTestingApiFactory factory, IUserContext userContext = null)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider
            .GetRequiredService<HomeAppContext>();
        UserContext = userContext;
    }

    public void Dispose() => _scope.Dispose();
}
