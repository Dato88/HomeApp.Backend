using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class CategoryGroupTests : BaseFinanceCommandsTest
{
    public CategoryGroupTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateCategoryGroup_ShouldCreateWithTargetPercent()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var categoryGroup = new CategoryGroup
        {
            HouseholdId = household.HouseholdId,
            Name = "Wohnen",
            CategoryGroupType = CategoryType.Expense,
            TargetPercent = 30
        };

        // Act
        var result = await CategoryGroupCommands.CreateCategoryGroupAsync(categoryGroup, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.CategoryGroups.FindAsync(result.Value);
        created.Should().NotBeNull();
        created!.Name.Should().Be("Wohnen");
        created.CategoryGroupType.Should().Be(CategoryType.Expense);
        created.TargetPercent.Should().Be(30);
        created.CreatedById.Should().Be(ExecutionContext.PersonId);
    }

    [Fact]
    public async Task CreateCategoryGroup_ShouldReturnErrorWhenNotMember()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var categoryGroup = new CategoryGroup
        {
            HouseholdId = foreignHousehold.HouseholdId, Name = "Hack", CategoryGroupType = CategoryType.Expense
        };

        // Act
        var result = await CategoryGroupCommands.CreateCategoryGroupAsync(categoryGroup, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.CategoryGroupCreateFailedWithMessage("HouseholdId is invalid"));
    }

    [Fact]
    public async Task CreateCategoryGroup_ShouldReturnErrorWhenNameExists()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var existing = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId,
            ExecutionContext.PersonId);
        var duplicate = new CategoryGroup
        {
            HouseholdId = household.HouseholdId, Name = existing.Name, CategoryGroupType = CategoryType.Expense
        };

        // Act
        var result = await CategoryGroupCommands.CreateCategoryGroupAsync(duplicate, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.CategoryGroupCreateFailedWithMessage("Name already exists"));
    }

    [Fact]
    public async Task UpdateCategoryGroup_ShouldUpdateNameTypeAndTargetPercent()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var categoryGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId,
            ExecutionContext.PersonId, CategoryType.Expense, 10);

        // Act
        var result = await CategoryGroupCommands.UpdateCategoryGroupAsync(categoryGroup.CategoryGroupId,
            "Sparen", CategoryType.Expense, 30, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.CategoryGroups.FindAsync(categoryGroup.CategoryGroupId);
        updated!.Name.Should().Be("Sparen");
        updated.TargetPercent.Should().Be(30);
    }

    [Fact]
    public async Task UpdateCategoryGroup_ShouldReturnErrorWhenNotMember()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var foreignGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(foreignHousehold.HouseholdId,
            otherPerson.PersonId);

        // Act
        var result = await CategoryGroupCommands.UpdateCategoryGroupAsync(foreignGroup.CategoryGroupId,
            "Hack", CategoryType.Expense, null, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(
            FinanceErrors.CategoryGroupUpdateFailedWithMessage("CategoryGroupId is invalid"));
    }

    [Fact]
    public async Task DeleteCategoryGroup_ShouldKeepCategoriesUngrouped()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var categoryGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId,
            ExecutionContext.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId, CategoryType.Expense, categoryGroup.CategoryGroupId);

        // Act
        var result = await CategoryGroupCommands.DeleteCategoryGroupAsync(categoryGroup.CategoryGroupId,
            CancellationToken.None);

        // Assert: the category survives, only the group link is cleared
        result.IsSuccess.Should().BeTrue();
        var reloaded = await DbContext.Categories
            .AsNoTracking()
            .SingleAsync(c => c.CategoryId == category.CategoryId);
        reloaded.CategoryGroupId.Should().BeNull();
    }

    [Fact]
    public async Task GetCategoryGroups_ShouldReturnOnlyHouseholdGroups()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var ownGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId,
            ExecutionContext.PersonId);
        await FinanceDataSeeder.GenereateDummyCategoryGroup(foreignHousehold.HouseholdId, otherPerson.PersonId);

        // Act
        var ownResult = await CategoryGroupQueries.GetCategoryGroupsAsync(household.HouseholdId,
            CancellationToken.None);
        var foreignResult = await CategoryGroupQueries.GetCategoryGroupsAsync(foreignHousehold.HouseholdId,
            CancellationToken.None);

        // Assert
        ownResult.IsSuccess.Should().BeTrue();
        ownResult.Value.Select(g => g.CategoryGroupId).Should().Contain(ownGroup.CategoryGroupId);
        foreignResult.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnErrorWhenGroupBelongsToOtherHousehold()
    {
        // Arrange: both households are mine, but the group lives in the other one
        var householdA = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var householdB = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var groupInB = await FinanceDataSeeder.GenereateDummyCategoryGroup(householdB.HouseholdId,
            ExecutionContext.PersonId);
        var category = new Category
        {
            HouseholdId = householdA.HouseholdId,
            Name = "Lebensmittel",
            CategoryType = CategoryType.Expense,
            CategoryGroupId = groupInB.CategoryGroupId
        };

        // Act
        var result = await CategoryCommands.CreateCategoryAsync(category, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.CategoryCreateFailedWithMessage("CategoryGroupId is invalid"));
    }

    [Fact]
    public async Task UpdateCategory_ShouldAssignGroupOfSameHousehold()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var categoryGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId,
            ExecutionContext.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);

        // Act
        var result = await CategoryCommands.UpdateCategoryAsync(category.CategoryId, category.Name,
            CategoryType.Expense, categoryGroup.CategoryGroupId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.Categories
            .AsNoTracking()
            .SingleAsync(c => c.CategoryId == category.CategoryId);
        updated.CategoryGroupId.Should().Be(categoryGroup.CategoryGroupId);
    }
}
