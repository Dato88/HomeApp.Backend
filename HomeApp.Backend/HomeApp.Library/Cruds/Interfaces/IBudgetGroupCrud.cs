namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetGroupCrud
    {
        Task<BudgetGroup> CreateAsync(BudgetGroup budgetGroup);
        Task<bool> DeleteAsync(int id);
        Task<BudgetGroup> FindByIdAsync(int id);
        Task<IEnumerable<BudgetGroup>> GetAllAsync();
        Task<IEnumerable<BudgetGroup>> GetAllAsync(int userId);
        Task UpdateAsync(BudgetGroup budgetGroup);
    }
}