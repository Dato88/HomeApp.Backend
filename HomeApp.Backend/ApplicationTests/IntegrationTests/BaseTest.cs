using Application.Abstractions.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTests.IntegrationTests;

public class BaseTest : IClassFixture<UnitTestingApiFactory>
{
    private readonly IServiceScope _scope;
    protected readonly IHomeAppContext DbContext;

    protected BaseTest(UnitTestingApiFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider
            .GetRequiredService<IHomeAppContext>();
    }
}
