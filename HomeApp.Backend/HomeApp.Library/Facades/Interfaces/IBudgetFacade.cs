using HomeApp.Library.Models;

namespace HomeApp.Library.Facades.Interfaces
{
    public interface IBudgetFacade
    {
        Task<Budget> GetBudgetAsync(int userId);
    }
}