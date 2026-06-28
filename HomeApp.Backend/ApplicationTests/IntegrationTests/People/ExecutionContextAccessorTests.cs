using System.Security.Claims;
using Infrastructure;
using Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace ApplicationTests.IntegrationTests.People;

public class ExecutionContextAccessorTests
{
    [Fact]
    public void PersonId_ShouldReturnValueFromHttpContextItems()
    {
        const int expectedPersonId = 42;
        var httpContext = new DefaultHttpContext();
        httpContext.Items[DependencyInjection.PersonIdItemKey] = expectedPersonId;

        var accessor = CreateAccessor(httpContext);

        accessor.PersonId.Should().Be(expectedPersonId);
    }

    [Fact]
    public void PersonId_ShouldPreferWorkerOverride()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Items[DependencyInjection.PersonIdItemKey] = 42;

        var accessor = CreateAccessor(httpContext);
        accessor.SetWorkerPersonId(99);

        accessor.PersonId.Should().Be(99);
    }

    [Fact]
    public void KeycloakUserId_ShouldReadSubClaim()
    {
        var keycloakUserId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim("sub", keycloakUserId.ToString())
            ], authenticationType: "Test"))
        };

        var accessor = CreateAccessor(httpContext);

        accessor.KeycloakUserId.Should().Be(keycloakUserId);
    }

    [Fact]
    public void PersonId_ShouldThrow_WhenContextIsMissing()
    {
        var accessor = CreateAccessor(new DefaultHttpContext());

        var act = () => accessor.PersonId;

        act.Should().Throw<UnauthorizedAccessException>();
    }

    private static ExecutionContextAccessor CreateAccessor(HttpContext httpContext)
    {
        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
        return new ExecutionContextAccessor(httpContextAccessor);
    }
}
