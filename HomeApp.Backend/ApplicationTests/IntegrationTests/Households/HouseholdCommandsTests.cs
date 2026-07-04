using Domain.Entities.Households;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Households;

public class HouseholdCommandsTests : BaseHouseholdCommandsTest
{
    public HouseholdCommandsTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateHousehold_ShouldCreateHouseholdWithCreatorAsMember()
    {
        // Act
        var result = await HouseholdCommands.CreateHouseholdAsync("Familie Test", CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.Households
            .Include(h => h.Members)
            .SingleAsync(h => h.HouseholdId == result.Value);
        created.Name.Should().Be("Familie Test");
        created.Members.Should().ContainSingle(m => m.PersonId == ExecutionContext.PersonId);
    }

    [Fact]
    public async Task UpdateHousehold_ShouldRename()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);

        // Act
        var result = await HouseholdCommands.UpdateHouseholdAsync(household.HouseholdId, "Neuer Name",
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.Households.FindAsync(household.HouseholdId);
        updated!.Name.Should().Be("Neuer Name");
    }

    [Fact]
    public async Task UpdateHousehold_ShouldReturnErrorWhenNotMember()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);

        // Act
        var result = await HouseholdCommands.UpdateHouseholdAsync(household.HouseholdId, "Hack",
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(HouseholdErrors.UpdateFailedWithMessage("HouseholdId is invalid"));
    }

    [Fact]
    public async Task DeleteHousehold_ShouldDeleteHouseholdWithMembers()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);

        // Act
        var result = await HouseholdCommands.DeleteHouseholdAsync(household.HouseholdId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.Households.AnyAsync(h => h.HouseholdId == household.HouseholdId)).Should().BeFalse();
        (await DbContext.HouseholdMembers.AnyAsync(m => m.HouseholdId == household.HouseholdId)).Should().BeFalse();
    }

    [Fact]
    public async Task AddHouseholdMember_ShouldAddByPersonId()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var partner = await PeopleDataSeeder.SeedPersonAsync();

        // Act
        var result = await HouseholdCommands.AddHouseholdMemberAsync(household.HouseholdId, null,
            partner.PersonId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.HouseholdMembers.AnyAsync(m =>
            m.HouseholdId == household.HouseholdId && m.PersonId == partner.PersonId)).Should().BeTrue();
    }

    [Fact]
    public async Task AddHouseholdMember_ShouldAddByEmail()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var partner = await PeopleDataSeeder.SeedPersonAsync();

        // Act
        var result = await HouseholdCommands.AddHouseholdMemberAsync(household.HouseholdId, partner.Email,
            null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.HouseholdMembers.AnyAsync(m =>
            m.HouseholdId == household.HouseholdId && m.PersonId == partner.PersonId)).Should().BeTrue();
    }

    [Fact]
    public async Task AddHouseholdMember_ShouldReturnErrorWhenAlreadyMember()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId,
            partner.PersonId);

        // Act
        var result = await HouseholdCommands.AddHouseholdMemberAsync(household.HouseholdId, null,
            partner.PersonId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(HouseholdErrors.AddMemberFailedWithMessage("Person is already a member"));
    }

    [Fact]
    public async Task AddHouseholdMember_ShouldReturnErrorWhenCallerIsNotMember()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var partner = await PeopleDataSeeder.SeedPersonAsync();

        // Act
        var result = await HouseholdCommands.AddHouseholdMemberAsync(household.HouseholdId, null,
            partner.PersonId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(HouseholdErrors.AddMemberFailedWithMessage("HouseholdId is invalid"));
    }

    [Fact]
    public async Task AddHouseholdMember_ShouldReturnErrorWhenPersonNotFound()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);

        // Act
        var result = await HouseholdCommands.AddHouseholdMemberAsync(household.HouseholdId,
            "does-not-exist@example.com", null, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(HouseholdErrors.AddMemberFailedWithMessage("Person was not found"));
    }

    [Fact]
    public async Task RemoveHouseholdMember_ShouldRemoveMember()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId,
            partner.PersonId);

        // Act
        var result = await HouseholdCommands.RemoveHouseholdMemberAsync(household.HouseholdId, partner.PersonId,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.HouseholdMembers.AnyAsync(m =>
            m.HouseholdId == household.HouseholdId && m.PersonId == partner.PersonId)).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveHouseholdMember_ShouldReturnErrorWhenLastMember()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);

        // Act
        var result = await HouseholdCommands.RemoveHouseholdMemberAsync(household.HouseholdId,
            ExecutionContext.PersonId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(HouseholdErrors.RemoveMemberFailedWithMessage("The last member cannot be removed"));
    }
}
