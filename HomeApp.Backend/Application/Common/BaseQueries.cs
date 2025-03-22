using Infrastructure.Database;

namespace Application.Common;

public abstract class BaseQueries<T>(HomeAppContext dbContext) : BaseContext(dbContext)
{
    public abstract Task FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    public abstract Task GetAllAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    protected abstract IQueryable<T> ApplyIncludes(IQueryable<T> query, params string[] includes);
}
