using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds;

public abstract class BaseCommands<T>(HomeAppContext dbContext) : BaseContext(dbContext)
{
    public abstract Task<int> CreateAsync(T t, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(int id, CancellationToken cancellationToken);
    public abstract Task UpdateAsync(T t, CancellationToken cancellationToken);
}
