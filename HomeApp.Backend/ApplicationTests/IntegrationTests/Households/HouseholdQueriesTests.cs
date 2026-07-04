namespace ApplicationTests.IntegrationTests.Households;

public class HouseholdQueriesTests : BaseHouseholdCommandsTest
{
    public HouseholdQueriesTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task GetHouseholds_ShouldReturnOwnAndSharedHouseholds()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var ownHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var sharedHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(partner.PersonId,
            ExecutionContext.PersonId);
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(partner.PersonId);

        // Act
        var result = await HouseholdQueries.GetHouseholdsAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var ids = result.Value.Select(h => h.HouseholdId).ToList();
        ids.Should().Contain(ownHousehold.HouseholdId);
        ids.Should().Contain(sharedHousehold.HouseholdId);
        ids.Should().NotContain(foreignHousehold.HouseholdId);

        var shared = result.Value.Single(h => h.HouseholdId == sharedHousehold.HouseholdId);
        shared.Members.Should().HaveCount(2);
        shared.Members.Select(m => m.Person).Should().NotContainNulls();
    }
}
