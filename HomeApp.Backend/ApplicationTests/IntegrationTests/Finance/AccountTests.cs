using Application.Features.Finance.Accounts.Commands;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class AccountTests : BaseFinanceCommandsTest
{
    public AccountTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateAccount_ShouldCreateAccountWithNormalizedIban()
    {
        // Arrange
        var command = new CreateAccountCommand("Girokonto", "de44 5001 0517 5407 3249 31", null,
            AccountType.Checking, null, null, null);

        // Act
        var result = await AccountCommands.CreateAccountAsync((Account)command, [], CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.Accounts.FindAsync(result.Value);
        created.Should().NotBeNull();
        created!.Iban.Should().Be("DE44500105175407324931");
        created.CurrencyCode.Should().Be("EUR");
        created.PersonId.Should().Be(ExecutionContext.PersonId);
    }

    [Fact]
    public async Task CreateAccount_ShouldShareIntoOwnHousehold()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var command = new CreateAccountCommand("Gemeinschaftskonto", null, null, AccountType.Checking,
            null, null, [household.HouseholdId]);

        // Act
        var result = await AccountCommands.CreateAccountAsync((Account)command,
            [household.HouseholdId], CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.AccountHouseholds.AnyAsync(ah =>
            ah.AccountId == result.Value && ah.HouseholdId == household.HouseholdId)).Should().BeTrue();
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnErrorWhenSharingIntoForeignHousehold()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var command = new CreateAccountCommand("Konto", null, null, AccountType.Checking, null, null, null);

        // Act
        var result = await AccountCommands.CreateAccountAsync((Account)command,
            [foreignHousehold.HouseholdId], CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.AccountCreateFailedWithMessage(
            $"HouseholdId '{foreignHousehold.HouseholdId}' is invalid"));
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnErrorWhenNotOwner()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var account = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        var command = new UpdateAccountCommand(account.AccountId, "Hack", null, null, AccountType.Checking,
            null, null, true);

        // Act
        var result = await AccountCommands.UpdateAccountAsync((Account)command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.AccountUpdateFailedWithMessage("AccountId is invalid"));
    }

    [Fact]
    public async Task DeleteAccount_ShouldDeleteOwnAccountWithTransactions()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, ExecutionContext.PersonId,
            new DateOnly(2026, 3, 1), -42);

        // Act
        var result = await AccountCommands.DeleteAccountAsync(account.AccountId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.Accounts.AnyAsync(a => a.AccountId == account.AccountId)).Should().BeFalse();
        (await DbContext.Transactions.AnyAsync(t => t.AccountId == account.AccountId)).Should().BeFalse();
    }

    [Fact]
    public async Task ShareAccount_ShouldShareAndUnshare()
    {
        // Arrange
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        // Act
        var shareResult = await AccountCommands.ShareAccountAsync(account.AccountId, household.HouseholdId,
            CancellationToken.None);
        var doubleShareResult = await AccountCommands.ShareAccountAsync(account.AccountId,
            household.HouseholdId, CancellationToken.None);
        var unshareResult = await AccountCommands.UnshareAccountAsync(account.AccountId,
            household.HouseholdId, CancellationToken.None);

        // Assert
        shareResult.IsSuccess.Should().BeTrue();
        doubleShareResult.IsFailure.Should().BeTrue();
        unshareResult.IsSuccess.Should().BeTrue();
        (await DbContext.AccountHouseholds.AnyAsync(ah => ah.AccountId == account.AccountId)).Should().BeFalse();
    }

    [Fact]
    public async Task ShareAccount_ShouldReturnErrorWhenNotOwner()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignAccount = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);

        // Act
        var result = await AccountCommands.ShareAccountAsync(foreignAccount.AccountId, household.HouseholdId,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.AccountShareFailedWithMessage("AccountId is invalid"));
    }

    [Fact]
    public async Task GetAccounts_ShouldReturnOwnAndSharedAccounts()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var sharedHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(partner.PersonId,
            ExecutionContext.PersonId);

        var ownAccount = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var partnerSharedAccount = await FinanceDataSeeder.GenereateDummyAccount(partner.PersonId, null,
            sharedHousehold.HouseholdId);
        var partnerPrivateAccount = await FinanceDataSeeder.GenereateDummyAccount(partner.PersonId);

        // Act
        var result = await AccountQueries.GetAccountsAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var ids = result.Value.Select(a => a.AccountId).ToList();
        ids.Should().Contain(ownAccount.AccountId);
        ids.Should().Contain(partnerSharedAccount.AccountId);
        ids.Should().NotContain(partnerPrivateAccount.AccountId);
    }
}
