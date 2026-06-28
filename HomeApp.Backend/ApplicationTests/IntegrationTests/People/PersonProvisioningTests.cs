using System.Security.Claims;
using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Features.People.Validations;
using ApplicationTests.IntegrationTests.TestData;
using Infrastructure.Features.People.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationTests.IntegrationTests.People;

public class PersonProvisioningTests : BaseTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;
    private readonly PersonIdCache _personIdCache;
    private readonly IAppLogger<PersonProvisioningService> _logger = new NoOpLogger();

    public PersonProvisioningTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        _personIdCache = new PersonIdCache(new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async Task EnsurePersonAsync_ShouldCreatePerson_WhenUserIsNew()
    {
        var keycloakUserId = Guid.NewGuid();
        var service = CreateProvisioningService();
        var user = CreateClaimsPrincipal(keycloakUserId);

        var personId = await service.EnsurePersonAsync(user, CancellationToken.None);

        personId.Should().BeGreaterThan(0);

        var person = await DbContext.People.FindAsync(personId);
        person.Should().NotBeNull();
        person!.UserId.Should().Be(keycloakUserId.ToString());
        person.Email.Should().Be($"{keycloakUserId:N}@example.com");
        person.FirstName.Should().Be("New");
        person.LastName.Should().Be("User");
        person.CreatedById.Should().Be(personId);
    }

    [Fact]
    public async Task EnsurePersonAsync_ShouldReturnExistingPerson_WhenUserAlreadyExists()
    {
        var existingPerson = await _peopleDataSeeder.SeedPersonAsync();
        var service = CreateProvisioningService();
        var user = CreateClaimsPrincipal(Guid.Parse(existingPerson.UserId), existingPerson.Email);

        var personId = await service.EnsurePersonAsync(user, CancellationToken.None);

        personId.Should().Be(existingPerson.PersonId);

        var personsWithSameUserId = DbContext.People.Count(p => p.UserId == existingPerson.UserId);
        personsWithSameUserId.Should().Be(1);
    }

    [Fact]
    public async Task EnsurePersonAsync_ShouldUseCache_OnSecondCall()
    {
        var keycloakUserId = Guid.NewGuid();
        var service = CreateProvisioningService();
        var user = CreateClaimsPrincipal(keycloakUserId);

        var firstPersonId = await service.EnsurePersonAsync(user, CancellationToken.None);
        var secondPersonId = await service.EnsurePersonAsync(user, CancellationToken.None);

        secondPersonId.Should().Be(firstPersonId);
        _personIdCache.TryGetPersonId(keycloakUserId.ToString(), out var cachedPersonId).Should().BeTrue();
        cachedPersonId.Should().Be(firstPersonId);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldInvalidateCache()
    {
        var keycloakUserId = Guid.NewGuid();
        var provisioningService = CreateProvisioningService();
        var user = CreateClaimsPrincipal(keycloakUserId);
        var personId = await provisioningService.EnsurePersonAsync(user, CancellationToken.None);

        _personIdCache.TryGetPersonId(keycloakUserId.ToString(), out _).Should().BeTrue();

        var personCommands = new Infrastructure.Features.People.Commands.PersonCommands(
            DbContext,
            Mock.Of<IPersonValidation>(),
            _personIdCache,
            Mock.Of<IAppLogger<Infrastructure.Features.People.Commands.PersonCommands>>());

        var deleteResult = await personCommands.DeletePersonAsync(personId, CancellationToken.None);

        deleteResult.IsSuccess.Should().BeTrue();
        _personIdCache.TryGetPersonId(keycloakUserId.ToString(), out _).Should().BeFalse();
    }

    private PersonProvisioningService CreateProvisioningService() =>
        new(DbContext, _personIdCache, _logger);

    private sealed class NoOpLogger : IAppLogger<PersonProvisioningService>
    {
        public void LogTrace(string message) { }
        public void LogDebug(string message) { }
        public void LogInformation(string message) { }
        public void LogWarning(string message) { }
        public void LogError(string message) { }
        public void LogCritical(string message) { }
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(
        Guid keycloakUserId,
        string? email = null)
    {
        email ??= $"{keycloakUserId:N}@example.com";

        var claims = new List<Claim>
        {
            new("sub", keycloakUserId.ToString()),
            new("email", email),
            new("given_name", "New"),
            new("family_name", "User"),
            new("preferred_username", $"user.{keycloakUserId:N}")
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Test"));
    }
}
