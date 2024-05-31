namespace HomeApp.Library.Validation.Interfaces
{
    public interface IBudgetValidation
    {
        Task ValidateBudgetCellForUserIdChangeAsync(int userId, int budgetCellId);
        Task ValidateBudgetColumnIdExistsAsync(int budgetColumnId);
        Task ValidateBudgetColumnIdExistsNotAsync(int budgetColumnId);
        Task ValidateBudgetColumnIndexAndNameAlreadyExistsAsync(int index, string name);
        Task ValidateBudgetGroupForUserIdChangeAsync(int userId, int budgetGroupId);
        Task ValidateBudgetGroupIdExistsAsync(int budgetGroupId);
        Task ValidateBudgetGroupIdExistsNotAsync(int budgetGroupId);
        Task ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(int index, string name);
        Task ValidateBudgetRowForUserIdChangeAsync(int userId, int budgetRowId);
        Task ValidateBudgetRowIdExistsAsync(int budgetRowId);
        Task ValidateBudgetRowIdExistsNotAsync(int budgetRowId);
        Task ValidateForEmptyStringAsync(string name);
        Task ValidateForPositiveIndexAsync(int index);
        Task ValidateForUserIdAsync(int userId);
    }
}