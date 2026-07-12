using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ITransactionCommands
{
    Task<Result<int>> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<Result<int>> UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<Result<int>> DeleteTransactionAsync(int transactionId, CancellationToken cancellationToken);

    Task<Result<int>> SetTransactionCategoryAsync(IReadOnlyList<int> transactionIds, int? categoryId,
        CancellationToken cancellationToken);

    Task<Result<ImportResult>> ImportTransactionsAsync(int accountId, IReadOnlyList<ParsedTransaction> items,
        TransactionSource source, CancellationToken cancellationToken);
}
