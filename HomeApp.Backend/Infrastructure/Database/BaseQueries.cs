namespace Infrastructure.Database;

public abstract class BaseQueries<T>(HomeAppContext dbContext) : BaseContext(dbContext)
{
    public abstract Task FindByIdAsync(T id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    public abstract Task GetAllAsync(T id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    protected abstract IQueryable<T> ApplyIncludes(IQueryable<T> query, params string[] includes);
}
