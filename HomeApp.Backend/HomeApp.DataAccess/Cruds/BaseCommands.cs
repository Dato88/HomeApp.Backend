using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds;

public abstract class BaseCommands<T>(HomeAppContext context) : BaseContext(context)
{
    public abstract Task CreateAsync(T t, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(int id, CancellationToken cancellationToken);
    public abstract Task UpdateAsync(T t, CancellationToken cancellationToken);
}
