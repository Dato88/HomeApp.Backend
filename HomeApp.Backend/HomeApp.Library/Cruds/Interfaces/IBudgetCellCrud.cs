namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetCellCrud
    {
        /// <summary>
        /// Creates a new BudgetCell.
        /// </summary>
        /// <param name="budgetCell">The BudgetCell to create.</param>
        /// <returns>The created BudgetCell.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetCell is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when UserId, BudgetRowId, BudgetColumnId, or BudgetGroupId does not exist.
        /// </exception>
        Task<BudgetCell> CreateAsync(BudgetCell budgetCell);

        /// <summary>
        /// Deletes a BudgetCell by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetCell to delete.</param>
        /// <returns>True if the operation is successful.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetCell with the given id is not found.
        /// </exception>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Finds a BudgetCell by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetCell to find.</param>
        /// <returns>The found BudgetCell.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetCell with the given id is not found.
        /// </exception>
        Task<BudgetCell> FindByIdAsync(int id);

        /// <summary>
        /// Gets all BudgetCells.
        /// </summary>
        /// <returns>A list of all BudgetCells.</returns>
        Task<IEnumerable<BudgetCell>> GetAllAsync();

        /// <summary>
        /// Gets all BudgetCells for a specific user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of all BudgetCells for the specified user.</returns>
        Task<IEnumerable<BudgetCell>> GetAllAsync(int userId);

        /// <summary>
        /// Updates a BudgetCell.
        /// </summary>
        /// <param name="budgetCell">The BudgetCell to update.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetCell is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when UserId, BudgetRowId, BudgetColumnId, or BudgetGroupId does not exist or when the BudgetCell with the given id is not found.
        /// </exception>
        Task UpdateAsync(BudgetCell budgetCell);

    }
}