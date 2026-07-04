using Domain.Entities.Finance;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ITransactionCommands
{
    Task<Result<int>> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<Result<int>> UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<Result<int>> DeleteTransactionAsync(int transactionId, CancellationToken cancellationToken);

    Task<Result<int>> SetTransactionCategoryAsync(int transactionId, int? categoryId,
        CancellationToken cancellationToken);
}
