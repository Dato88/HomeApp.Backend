namespace HomeApp.Library.Crud
{
    public abstract class BaseCrud<T>(HomeAppContext context, IBudgetValidation budgetValidation) : BaseContext(context)
    {
        protected readonly IBudgetValidation _budgetValidation = budgetValidation;

        public abstract Task CreateAsync(T t);
        public abstract Task DeleteAsync(int id);
        public abstract Task FindByIdAsync(int id);
        public abstract Task GetAllAsync();
        public abstract Task UpdateAsync(T t);
    }
}
