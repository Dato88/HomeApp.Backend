using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Database;
using Infrastructure.Features.Finance.Import;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Commands;

public sealed class TransactionCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ITransactionCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateTransactionAsync(Transaction transaction,
        CancellationToken cancellationToken)
    {
        var accountIsOwned = _dbContext.Accounts.Any(a =>
            a.AccountId == transaction.AccountId && a.PersonId == _executionContext.PersonId);

        if (!accountIsOwned)
            return Result.Failure<int>(FinanceErrors.TransactionCreateFailedWithMessage("AccountId is invalid"));

        var categoryError = ValidateTransactionCategory(transaction.AccountId, transaction.CategoryId);

        if (categoryError is not null)
            return Result.Failure<int>(FinanceErrors.TransactionCreateFailedWithMessage(categoryError));

        transaction.CreatedById = _executionContext.PersonId;

        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(transaction.TransactionId);
    }

    public async Task<Result<int>> UpdateTransactionAsync(Transaction transaction,
        CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Transactions.SingleOrDefaultAsync(t =>
            t.TransactionId == transaction.TransactionId &&
            t.Account.PersonId == _executionContext.PersonId, cancellationToken);

        if (existing == null)
            return Result.Failure<int>(
                FinanceErrors.TransactionUpdateFailedWithMessage("TransactionId is invalid"));

        existing.BookingDate = transaction.BookingDate;
        existing.ValueDate = transaction.ValueDate;
        existing.Amount = transaction.Amount;
        existing.CounterpartyName = transaction.CounterpartyName;
        existing.CounterpartyIban = transaction.CounterpartyIban;
        existing.Purpose = transaction.Purpose;
        existing.UpdatedById = _executionContext.PersonId;
        existing.UpdatedAt = DateTime.UtcNow;

        _dbContext.Transactions.Update(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(existing.TransactionId);
    }

    public async Task<Result<int>> DeleteTransactionAsync(int transactionId, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.Transactions.SingleOrDefaultAsync(t =>
            t.TransactionId == transactionId &&
            t.Account.PersonId == _executionContext.PersonId, cancellationToken);

        if (transaction == null)
            return Result.Failure<int>(
                FinanceErrors.TransactionDeleteFailedWithMessage("TransactionId is invalid"));

        _dbContext.Transactions.Remove(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(transactionId);
    }

    public async Task<Result<int>> SetTransactionCategoryAsync(IReadOnlyList<int> transactionIds, int? categoryId,
        CancellationToken cancellationToken)
    {
        var ids = transactionIds.Distinct().ToList();

        // Categorizing is allowed for the owner and for members of households the account is shared into
        var transactions = await _dbContext.Transactions
            .Where(t =>
                ids.Contains(t.TransactionId) &&
                (t.Account.PersonId == _executionContext.PersonId ||
                 t.Account.AccountHouseholds.Any(ah =>
                     ah.Household.Members.Any(m => m.PersonId == _executionContext.PersonId))))
            .ToListAsync(cancellationToken);

        // All-or-nothing: an unknown or invisible id fails the whole batch without leaking which one
        if (transactions.Count != ids.Count)
            return Result.Failure<int>(
                FinanceErrors.TransactionUpdateFailedWithMessage("TransactionIds are invalid"));

        // A bulk selection may span accounts; the category must be valid for every single one
        foreach (var accountId in transactions.Select(t => t.AccountId).Distinct())
        {
            var categoryError = ValidateTransactionCategory(accountId, categoryId);

            if (categoryError is not null)
                return Result.Failure<int>(FinanceErrors.TransactionUpdateFailedWithMessage(categoryError));
        }

        foreach (var transaction in transactions)
        {
            transaction.CategoryId = categoryId;
            transaction.UpdatedById = _executionContext.PersonId;
            transaction.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(transactions.Count);
    }

    public async Task<Result<ImportResult>> ImportTransactionsAsync(int accountId,
        IReadOnlyList<ParsedTransaction> items, TransactionSource source, CancellationToken cancellationToken)
    {
        var accountIsOwned = _dbContext.Accounts.Any(a =>
            a.AccountId == accountId && a.PersonId == _executionContext.PersonId);

        if (!accountIsOwned)
            return Result.Failure<ImportResult>(FinanceErrors.ImportFailedWithMessage("AccountId is invalid"));

        // Same canonical hash for every import format; occurrence counter distinguishes identical
        // bookings within this file
        var occurrences = new Dictionary<string, int>();
        var candidates = new List<(ParsedTransaction Item, string Hash)>();

        foreach (var item in items)
        {
            var key = TransactionHashCalculator.ComputeKey(accountId, item);
            occurrences.TryGetValue(key, out var occurrence);
            occurrences[key] = occurrence + 1;
            candidates.Add((item, TransactionHashCalculator.ComputeHash(key, occurrence)));
        }

        var hashes = candidates.Select(c => c.Hash).ToList();

        var existingHashes = (await _dbContext.Transactions
                .Where(t => t.AccountId == accountId && t.ImportHash != null && hashes.Contains(t.ImportHash))
                .Select(t => t.ImportHash!)
                .ToListAsync(cancellationToken))
            .ToHashSet();

        var imported = 0;

        foreach (var (item, hash) in candidates)
        {
            if (existingHashes.Contains(hash))
                continue;

            _dbContext.Transactions.Add(new Transaction
            {
                AccountId = accountId,
                BookingDate = item.BookingDate,
                ValueDate = item.ValueDate,
                Amount = item.Amount,
                CounterpartyName = Truncate(item.CounterpartyName, 200),
                CounterpartyIban = item.CounterpartyIban is null
                    ? null
                    : Truncate(Domain.ValueObjects.Iban.Normalize(item.CounterpartyIban), 34),
                Purpose = Truncate(item.Purpose, 500),
                BankReference = Truncate(item.BankReference, 100),
                ImportHash = hash,
                Source = source,
                CreatedById = _executionContext.PersonId
            });

            imported++;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new ImportResult(imported, items.Count - imported));
    }

    private static string? Truncate(string? value, int maxLength) =>
        value is null || value.Length <= maxLength ? value : value[..maxLength];

    // Returns an error message or null. A transaction category must belong to a household the account is
    // shared into and the caller must be a member of that household, otherwise the E+A of that household
    // could be manipulated by outsiders or reference invisible data.
    private string? ValidateTransactionCategory(int accountId, int? categoryId)
    {
        if (!categoryId.HasValue)
            return null;

        var category = _dbContext.Categories
            .AsNoTracking()
            .SingleOrDefault(c => c.CategoryId == categoryId.Value);

        if (category == null)
            return "CategoryId is invalid";

        var callerIsMember = _dbContext.HouseholdMembers.Any(m =>
            m.HouseholdId == category.HouseholdId && m.PersonId == _executionContext.PersonId);

        if (!callerIsMember)
            return "CategoryId is invalid";

        var accountIsShared = _dbContext.AccountHouseholds.Any(ah =>
            ah.AccountId == accountId && ah.HouseholdId == category.HouseholdId);

        if (!accountIsShared)
            return "Account is not shared into the category's household";

        return null;
    }
}
