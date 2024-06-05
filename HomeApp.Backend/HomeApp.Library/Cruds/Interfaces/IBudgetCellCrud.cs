namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetCellCrud
    {
        Task<BudgetCell> CreateAsync(BudgetCell budgetCell);
        Task<bool> DeleteAsync(int id);
        Task<BudgetCell> FindByIdAsync(int id);
        Task<IEnumerable<BudgetCell>> GetAllAsync();
        Task<IEnumerable<BudgetCell>> GetAllAsync(int userId);
        Task UpdateAsync(BudgetCell budgetCell);
    }
}