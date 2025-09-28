using ApplicationTests.IntegrationTests.TestData;
using Domain.Entities.People;

namespace ApplicationTests.IntegrationTests.People.Commands;

internal class PersonCommandsDeleteTests : BaseCommonPersonTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;

    public PersonCommandsDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);

    [Fact]
    public async Task DeleteAsync_ShouldSucceed_WhenPersonExists()
    {
        var person = await _peopleDataSeeder.SeedPersonAsync();

        var result = await CommonPersonCommands.DeletePersonAsync(person.PersonId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var deleted = await DbContext.People.FindAsync(person.PersonId);
        deleted.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task DeleteAsync_ShouldFail_WhenIdIsInvalid(int invalidId)
    {
        var result = await CommonPersonCommands.DeletePersonAsync(invalidId, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(PersonErrors.DeleteFailed(invalidId));
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenPersonDoesNotExist()
    {
        var nonExistentId = 9999;

        var result = await CommonPersonCommands.DeletePersonAsync(nonExistentId, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(PersonErrors.NotFoundById(nonExistentId));
        result.Error.Description.Should().Contain(nonExistentId.ToString());
    }
}
