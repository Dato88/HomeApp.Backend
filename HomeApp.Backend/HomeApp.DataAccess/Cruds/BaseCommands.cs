using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds;

public abstract class BaseCommands<T>(HomeAppContext dbContext) : BaseContext(dbContext)
{
    public abstract Task<T> CreateAsync(T t, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(int id, CancellationToken cancellationToken);
    public abstract Task<T> UpdateAsync(T t, CancellationToken cancellationToken);
}
