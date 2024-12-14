namespace HomeApp.Library.Cruds;

public abstract class BaseCrud<T>(HomeAppContext context, IBudgetValidation budgetValidation) : BaseContext(context)
{
    protected readonly IBudgetValidation _budgetValidation = budgetValidation;

    public abstract Task CreateAsync(T t, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(int id, CancellationToken cancellationToken);
    public abstract Task FindByIdAsync(int id, CancellationToken cancellationToken);
    public abstract Task GetAllAsync(CancellationToken cancellationToken);
    public abstract Task UpdateAsync(T t, CancellationToken cancellationToken);
}
