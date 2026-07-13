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
            PaymentPartnerName = "REWE",
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
        var result = await TransactionCommands.SetTransactionCategoryAsync([transaction.TransactionId],
            category.CategoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
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
        var result = await TransactionCommands.SetTransactionCategoryAsync([transaction.TransactionId],
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
        var result = await TransactionCommands.SetTransactionCategoryAsync([transaction.TransactionId], null,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionUpdateFailedWithMessage("TransactionIds are invalid"));
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldCategorizeManyAtOnce()
    {
        // Arrange: recurring bookings selected via multi-select and categorized in one call
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId, null,
            household.HouseholdId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);

        var transactionIds = new List<int>();
        for (var month = 1; month <= 3; month++)
        {
            var transaction = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
                ExecutionContext.PersonId, new DateOnly(2026, month, 1), -950);
            transactionIds.Add(transaction.TransactionId);
        }

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(transactionIds,
            category.CategoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(3);
        var categorized = await DbContext.Transactions.AsNoTracking()
            .Where(t => transactionIds.Contains(t.TransactionId))
            .ToListAsync();
        categorized.Should().OnlyContain(t => t.CategoryId == category.CategoryId);
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldFailWholeBatchWhenOneTransactionIsForeign()
    {
        // Arrange: one own and one foreign transaction in the same batch
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var ownAccount = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId, null,
            household.HouseholdId);
        var foreignAccount = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        var ownTransaction = await FinanceDataSeeder.GenereateDummyTransaction(ownAccount.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 2, 1), -20);
        var foreignTransaction = await FinanceDataSeeder.GenereateDummyTransaction(foreignAccount.AccountId,
            otherPerson.PersonId, new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(
            [ownTransaction.TransactionId, foreignTransaction.TransactionId], category.CategoryId,
            CancellationToken.None);

        // Assert: all-or-nothing, nothing was persisted
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionUpdateFailedWithMessage("TransactionIds are invalid"));
        var own = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.TransactionId == ownTransaction.TransactionId);
        own.CategoryId.Should().BeNull();
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldFailWhenCategoryInvalidForOneAccountOfBatch()
    {
        // Arrange: batch spans two accounts, but only one is shared into the category's household
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var sharedAccount = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId, null,
            household.HouseholdId);
        var unsharedAccount = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        var sharedTransaction = await FinanceDataSeeder.GenereateDummyTransaction(sharedAccount.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 2, 1), -20);
        var unsharedTransaction = await FinanceDataSeeder.GenereateDummyTransaction(unsharedAccount.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 2, 1), -20);

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(
            [sharedTransaction.TransactionId, unsharedTransaction.TransactionId], category.CategoryId,
            CancellationToken.None);

        // Assert: all-or-nothing, the shared transaction stays uncategorized too
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.TransactionUpdateFailedWithMessage(
            "Account is not shared into the category's household"));
        var shared = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.TransactionId == sharedTransaction.TransactionId);
        shared.CategoryId.Should().BeNull();
    }

    [Fact]
    public async Task SetTransactionCategory_ShouldClearCategoryForBatch()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId, null,
            household.HouseholdId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId,
            ExecutionContext.PersonId);
        var first = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 2, 1), -20, category.CategoryId);
        var second = await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId,
            ExecutionContext.PersonId, new DateOnly(2026, 3, 1), -20, category.CategoryId);

        // Act
        var result = await TransactionCommands.SetTransactionCategoryAsync(
            [first.TransactionId, second.TransactionId], null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(2);
        var cleared = await DbContext.Transactions.AsNoTracking()
            .Where(t => t.TransactionId == first.TransactionId || t.TransactionId == second.TransactionId)
            .ToListAsync();
        cleared.Should().OnlyContain(t => t.CategoryId == null);
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
            null, null, null, 1, 4, CancellationToken.None);
        var rangeResult = await TransactionQueries.GetTransactionsAsync(account.AccountId,
            new DateOnly(2026, 2, 1), new DateOnly(2026, 3, 31), null, null, null, null, 1, 50,
            CancellationToken.None);

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
            null, null, null, 1, 50, CancellationToken.None);

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
            null, null, 1, 50, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task GetTransactions_ShouldFilterByPaymentPartnerIbanWithPagination()
    {
        // Arrange: 3 bookings for the target partner, 1 for another one, 1 without IBAN
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        const string targetIban = "DE02120300000000202051";

        for (var month = 1; month <= 3; month++)
            await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
                new DateOnly(2026, month, 5), -950, null, targetIban);

        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
            new DateOnly(2026, 1, 10), -50, null, "DE89370400440532013000");
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
            new DateOnly(2026, 1, 15), -10.76m);

        // Act
        var filtered = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null,
            null, targetIban, null, 1, 2, CancellationToken.None);
        var noMatch = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null,
            null, "DE44500105175407324931", null, 1, 50, CancellationToken.None);

        // Assert: totalCount reflects the IBAN matches, not all account bookings
        filtered.IsSuccess.Should().BeTrue();
        filtered.Value.TotalCount.Should().Be(3);
        filtered.Value.Items.Should().HaveCount(2);
        filtered.Value.Items.Should().OnlyContain(t => t.PaymentPartnerIban == targetIban);

        noMatch.Value.TotalCount.Should().Be(0);
        noMatch.Value.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactions_ShouldNormalizePaymentPartnerIbanOnBothSides()
    {
        // Arrange: manually created bookings may store the IBAN unnormalized
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
            new DateOnly(2026, 3, 5), -950, null, "de02 1203 0000 0000 2020 51");

        // Act: differently formatted parameter (lowercase, other spacing)
        var result = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null,
            null, " De021203 0000 0000 202051 ", null, 1, 50, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task GetTransactions_ShouldBehaveUnchangedWithoutPaymentPartnerIban()
    {
        // Arrange: bookings without partner IBAN must still appear in the unfiltered list
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
            new DateOnly(2026, 3, 5), -950, null, "DE02120300000000202051");
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
            new DateOnly(2026, 3, 13), -10.76m);

        // Act
        var result = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null,
            null, null, null, 1, 50, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(2);
        result.Value.Items.Should().Contain(t => t.PaymentPartnerIban == null);
    }
}
