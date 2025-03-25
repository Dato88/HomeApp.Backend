using Application.Abstractions.Data;

namespace Application.Common;

public abstract class BaseCommands<T>(IHomeAppContext dbContext) : BaseContext(dbContext)
{
    public abstract Task<int> CreateAsync(T t, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(int id, CancellationToken cancellationToken);
    public abstract Task UpdateAsync(T t, CancellationToken cancellationToken);
}
