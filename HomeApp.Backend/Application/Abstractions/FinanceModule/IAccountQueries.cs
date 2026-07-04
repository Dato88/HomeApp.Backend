using Domain.Entities.Finance;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface IAccountQueries
{
    Task<Result<List<Account>>> GetAccountsAsync(CancellationToken cancellationToken);
}
