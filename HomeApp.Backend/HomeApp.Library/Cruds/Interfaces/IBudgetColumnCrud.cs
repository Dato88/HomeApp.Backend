namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetColumnCrud
    {
        Task<BudgetColumn> CreateAsync(BudgetColumn budgetColumn);
        Task<bool> DeleteAsync(int id);
        Task<BudgetColumn> FindByIdAsync(int id);
        Task<IEnumerable<BudgetColumn>> GetAllAsync();
        Task UpdateAsync(BudgetColumn budgetColumn);
    }
}