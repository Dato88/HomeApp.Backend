using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds;

public abstract class BaseCrud<T>(HomeAppContext context) : BaseContext(context)
{
    public abstract Task CreateAsync(T t, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(int id, CancellationToken cancellationToken);

    public abstract Task FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    public abstract Task GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    public abstract Task UpdateAsync(T t, CancellationToken cancellationToken);

    protected abstract IQueryable<T> ApplyIncludes(IQueryable<T> query, params string[] includes);
}
