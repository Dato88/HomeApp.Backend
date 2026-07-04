using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ITransactionQueries
{
    Task<Result<TransactionPage>> GetTransactionsAsync(int accountId, DateOnly? from, DateOnly? to,
        int? categoryId, bool? uncategorized, int page, int pageSize, CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<CategoryMonthAmount>>> GetMonthlyCategoryTotalsAsync(int householdId, int year,
        CancellationToken cancellationToken);
}
