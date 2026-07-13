using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Commands;

public sealed class AccountCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : IAccountCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateAccountAsync(Account account, IReadOnlyList<int> householdIds,
        CancellationToken cancellationToken)
    {
        foreach (var householdId in householdIds.Distinct())
        {
            var isMember = _dbContext.HouseholdMembers.Any(m =>
                m.HouseholdId == householdId && m.PersonId == _executionContext.PersonId);

            if (!isMember)
                return Result.Failure<int>(
                    FinanceErrors.AccountCreateFailedWithMessage($"HouseholdId '{householdId}' is invalid"));
        }

        if (account.Iban is not null)
        {
            var ibanExists = _dbContext.Accounts.Any(a =>
                a.PersonId == _executionContext.PersonId && a.Iban == account.Iban);

            if (ibanExists)
                return Result.Failure<int>(FinanceErrors.AccountCreateFailedWithMessage("Iban already exists"));
        }

        account.PersonId = _executionContext.PersonId;
        account.CreatedById = _executionContext.PersonId;

        foreach (var householdId in householdIds.Distinct())
            account.AccountHouseholds.Add(new AccountHousehold
            {
                HouseholdId = householdId, CreatedById = _executionContext.PersonId
            });

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(account.AccountId);
    }

    public async Task<Result<int>> UpdateAccountAsync(Account account, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Accounts.SingleOrDefaultAsync(a =>
            a.AccountId == account.AccountId &&
            a.PersonId == _executionContext.PersonId, cancellationToken);

        if (existing == null)
            return Result.Failure<int>(FinanceErrors.AccountUpdateFailedWithMessage("AccountId is invalid"));

        if (account.Iban is not null)
        {
            var ibanExists = _dbContext.Accounts.Any(a =>
                a.PersonId == _executionContext.PersonId &&
                a.Iban == account.Iban &&
                a.AccountId != account.AccountId);

            if (ibanExists)
                return Result.Failure<int>(FinanceErrors.AccountUpdateFailedWithMessage("Iban already exists"));
        }

        existing.Name = account.Name;
        existing.Iban = account.Iban;
        existing.Bic = account.Bic;
        existing.AccountType = account.AccountType;
        existing.CurrencyCode = account.CurrencyCode;
        existing.Description = account.Description;
        existing.IsActive = account.IsActive;
        existing.DeactivatedFrom = account.DeactivatedFrom;
        existing.UpdatedById = _executionContext.PersonId;
        existing.UpdatedAt = DateTime.UtcNow;

        _dbContext.Accounts.Update(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(existing.AccountId);
    }

    public async Task<Result<int>> DeleteAccountAsync(int accountId, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts.SingleOrDefaultAsync(a =>
            a.AccountId == accountId &&
            a.PersonId == _executionContext.PersonId, cancellationToken);

        if (account == null)
            return Result.Failure<int>(FinanceErrors.AccountDeleteFailedWithMessage("AccountId is invalid"));

        _dbContext.Accounts.Remove(account);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(accountId);
    }

    public async Task<Result<int>> ShareAccountAsync(int accountId, int householdId,
        CancellationToken cancellationToken)
    {
        var accountIsOwned = _dbContext.Accounts.Any(a =>
            a.AccountId == accountId && a.PersonId == _executionContext.PersonId);

        if (!accountIsOwned)
            return Result.Failure<int>(FinanceErrors.AccountShareFailedWithMessage("AccountId is invalid"));

        var isMember = _dbContext.HouseholdMembers.Any(m =>
            m.HouseholdId == householdId && m.PersonId == _executionContext.PersonId);

        if (!isMember)
            return Result.Failure<int>(FinanceErrors.AccountShareFailedWithMessage("HouseholdId is invalid"));

        var alreadyShared = _dbContext.AccountHouseholds.Any(ah =>
            ah.AccountId == accountId && ah.HouseholdId == householdId);

        if (alreadyShared)
            return Result.Failure<int>(
                FinanceErrors.AccountShareFailedWithMessage("Account is already shared into this household"));

        var accountHousehold = new AccountHousehold
        {
            AccountId = accountId, HouseholdId = householdId, CreatedById = _executionContext.PersonId
        };

        _dbContext.AccountHouseholds.Add(accountHousehold);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(accountHousehold.AccountHouseholdId);
    }

    public async Task<Result<int>> UnshareAccountAsync(int accountId, int householdId,
        CancellationToken cancellationToken)
    {
        var accountIsOwned = _dbContext.Accounts.Any(a =>
            a.AccountId == accountId && a.PersonId == _executionContext.PersonId);

        if (!accountIsOwned)
            return Result.Failure<int>(FinanceErrors.AccountShareFailedWithMessage("AccountId is invalid"));

        var accountHousehold = await _dbContext.AccountHouseholds.SingleOrDefaultAsync(ah =>
            ah.AccountId == accountId && ah.HouseholdId == householdId, cancellationToken);

        if (accountHousehold == null)
            return Result.Failure<int>(
                FinanceErrors.AccountShareFailedWithMessage("Account is not shared into this household"));

        _dbContext.AccountHouseholds.Remove(accountHousehold);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(accountHousehold.AccountHouseholdId);
    }
}
