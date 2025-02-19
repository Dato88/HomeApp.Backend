using Microsoft.Extensions.DependencyInjection;
using IServiceScope = Microsoft.Extensions.DependencyInjection.IServiceScope;

namespace HomeApp.DataAccess.Tests.Helper;

public class BaseTest : IClassFixture<UnitTestingApiFactory>
{
    private readonly IServiceScope _scope;
    protected readonly HomeAppContext DbContext;

    protected BaseTest(UnitTestingApiFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider
            .GetRequiredService<HomeAppContext>();
    }
}
