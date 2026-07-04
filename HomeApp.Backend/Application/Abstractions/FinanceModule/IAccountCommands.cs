using Domain.Entities.Finance;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface IAccountCommands
{
    Task<Result<int>> CreateAccountAsync(Account account, IReadOnlyList<int> householdIds,
        CancellationToken cancellationToken);

    Task<Result<int>> UpdateAccountAsync(Account account, CancellationToken cancellationToken);
    Task<Result<int>> DeleteAccountAsync(int accountId, CancellationToken cancellationToken);

    Task<Result<int>> ShareAccountAsync(int accountId, int householdId, CancellationToken cancellationToken);
    Task<Result<int>> UnshareAccountAsync(int accountId, int householdId, CancellationToken cancellationToken);
}
