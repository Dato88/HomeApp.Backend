using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Features.Finance.Commands;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class TransactionTests : BaseFinanceCommandsTest
{
    public TransactionTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateTransaction_ShouldCreateTransaction()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var transaction = new Transaction
        {
            AccountId = account.AccountId,
            BookingDate = new DateOnly(2026, 1, 15),
            Amount = -49.99m,
            CounterpartyName = "REWE",
            Purpose = "Einkauf",
            Source = TransactionSource.Manual
        };

        // Act
        var result = await TransactionCommands.CreateTransactionAsync(transaction, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.Transactions.FindAsync(result.Value);
        created.Should().NotBeNull();
        created!.Amount.Should().Be(-49.99m);
        created.CreatedById.Should().Be(ExecutionContext.PersonId);
    }

    [Fact]
    public async Task CreateTransaction_ShouldReturnErrorWhenNotOwner()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignAccount = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        var transaction = new Transaction
        {
            AccountId = foreignAccount.AccountId, BookingDate = new DateOnly(2026, 1, 15), Amount = -1
        };

        // Act
        var result = await TransactionCommands.CreateTransactionAsync(transaction, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionCreateFailedWithMessage("AccountId is invalid"));
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldAllowHouseholdMember()
    {
        // Arrange: partner owns the account, shared into the common household; we categorize
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(partner.PersonId,
            ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(partner.PersonId, null,
            household.HouseholdId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, partner.PersonId);
        var transaction = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
            partner.PersonId, new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(transaction.TransactionId,
            category.CategoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.TransactionId == transaction.TransactionId);
        updated.CategoryId.Should().Be(category.CategoryId);
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldReturnErrorWhenAccountNotSharedIntoCategoryHousehold()
    {
        // Arrange: account not shared anywhere, category lives in own household
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        var transaction = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(transaction.TransactionId,
            category.CategoryId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionUpdateFailedWithMessage(
            "Account is not shared into the category's household"));
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldReturnErrorForStranger()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignAccount = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        var transaction = await FinanceDataSeeder.GenereateDummyTransaction(foreignAccount.AccountId,
            otherPerson.PersonId, new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(transaction.TransactionId, null,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionUpdateFailedWithMessage("TransactionId is invalid"));
    }

    [Fact]
    public async Task DeleteTransaction_ShouldReturnErrorForHouseholdMemberWhoIsNotOwner()
    {
        // Arrange: shared household member may read, but only the owner may delete
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(partner.PersonId,
            ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(partner.PersonId, null,
            household.HouseholdId);
        var transaction = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
            partner.PersonId, new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionCommands.DeleteTransactionAsync(transaction.TransactionId,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionDeleteFailedWithMessage("TransactionId is invalid"));
    }

    [Fact]
    public async Task GetTransactions_ShouldFilterAndPaginate()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        for (var month = 1; month <= 6; month++)
            await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
                new DateOnly(2026, month, 10), -10 * month);

        // Act
        var allResult = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null,
            null, 1, 4, CancellationToken.None);
        var rangeResult = await TransactionQueries.GetTransactionsAsync(account.AccountId,
            new DateOnly(2026, 2, 1), new DateOnly(2026, 3, 31), null, null, 1, 50, CancellationToken.None);

        // Assert
        allResult.IsSuccess.Should().BeTrue();
        allResult.Value.TotalCount.Should().Be(6);
        allResult.Value.Items.Should().HaveCount(4);
        allResult.Value.Items.First().BookingDate.Should().Be(new DateOnly(2026, 6, 10));

        rangeResult.Value.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetTransactions_ShouldReturnNothingForStranger()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignAccount = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        await FinanceDataSeeder.GenereateDummyTransaction(foreignAccount.AccountId, otherPerson.PersonId,
            new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionQueries.GetTransactionsAsync(foreignAccount.AccountId, null, null, null,
            null, 1, 50, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(0);
        result.Value.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactions_ShouldAllowHouseholdMemberToRead()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(partner.PersonId,
            ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(partner.PersonId, null,
            household.HouseholdId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, partner.PersonId,
            new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null, null,
            1, 50, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(1);
    }
}
