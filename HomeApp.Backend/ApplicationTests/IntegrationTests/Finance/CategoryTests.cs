using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class CategoryTests : BaseFinanceCommandsTest
{
    public CategoryTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateCategory_ShouldCreateCategory()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var category = new Category
        {
            HouseholdId = household.HouseholdId, Name = "Lebensmittel", CategoryType = CategoryType.Expense
        };

        // Act
        var result = await CategoryCommands.CreateCategoryAsync(category, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.Categories.FindAsync(result.Value);
        created.Should().NotBeNull();
        created!.Name.Should().Be("Lebensmittel");
        created.CategoryType.Should().Be(CategoryType.Expense);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnErrorWhenNameExists()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var existing = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        var duplicate = new Category
        {
            HouseholdId = household.HouseholdId, Name = existing.Name, CategoryType = CategoryType.Expense
        };

        // Act
        var result = await CategoryCommands.CreateCategoryAsync(duplicate, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.CategoryCreateFailedWithMessage("Name already exists"));
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnErrorWhenNotMember()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var category = new Category
        {
            HouseholdId = foreignHousehold.HouseholdId, Name = "Hack", CategoryType = CategoryType.Expense
        };

        // Act
        var result = await CategoryCommands.CreateCategoryAsync(category, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.CategoryCreateFailedWithMessage("HouseholdId is invalid"));
    }

    [Fact]
    public async Task UpdateCategory_ShouldUpdateNameAndType()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);

        // Act
        var result = await CategoryCommands.UpdateCategoryAsync(category.CategoryId, "Gehalt",
            CategoryType.Income, null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.Categories.FindAsync(category.CategoryId);
        updated!.Name.Should().Be("Gehalt");
        updated.CategoryType.Should().Be(CategoryType.Income);
    }

    [Fact]
    public async Task DeleteCategory_ShouldKeepTransactionsUncategorized()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId, null,
            household.HouseholdId);
        var transaction = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 2, 15), -10, category.CategoryId);

        // Act
        var result = await CategoryCommands.DeleteCategoryAsync(category.CategoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var reloaded = await DbContext.Transactions
            .AsNoTracking()
            .SingleAsync(t => t.TransactionId == transaction.TransactionId);
        reloaded.CategoryId.Should().BeNull();
    }

    [Fact]
    public async Task GetCategories_ShouldReturnOnlyHouseholdCategories()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var ownCategory = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        await FinanceDataSeeder.GenereateDummyCategory(foreignHousehold.HouseholdId, otherPerson.PersonId);

        // Act
        var ownResult = await CategoryQueries.GetCategoriesAsync(household.HouseholdId, CancellationToken.None);
        var foreignResult = await CategoryQueries.GetCategoriesAsync(foreignHousehold.HouseholdId,
            CancellationToken.None);

        // Assert
        ownResult.IsSuccess.Should().BeTrue();
        ownResult.Value.Select(c => c.CategoryId).Should().Contain(ownCategory.CategoryId);
        foreignResult.Value.Should().BeEmpty();
    }
}
