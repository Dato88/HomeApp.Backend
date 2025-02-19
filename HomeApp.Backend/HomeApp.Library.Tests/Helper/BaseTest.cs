using Microsoft.Extensions.DependencyInjection;

namespace HomeApp.Library.Tests.Helper;

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
