using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ITransactionQueries
{
    Task<Result<TransactionPage>> GetTransactionsAsync(int accountId, DateOnly? from, DateOnly? to,
        int? categoryId, bool? uncategorized, string? counterpartyIban, int page, int pageSize,
        CancellationToken cancellationToken);
}
