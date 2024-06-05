namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetRowCrud
    {
        Task<BudgetRow> CreateAsync(BudgetRow budgetRow);
        Task DeleteAsync(int id);
        Task<BudgetRow> FindByIdAsync(int id);
        Task<IEnumerable<BudgetRow>> GetAllAsync();
        Task<IEnumerable<BudgetRow>> GetAllAsync(int userId);
        Task UpdateAsync(BudgetRow budgetRow);
    }
}