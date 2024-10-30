namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetRowCrud
    {
        /// <summary>
        /// Creates a new BudgetRow.
        /// </summary>
        /// <param name="budgetRow">The BudgetRow to create.</param>
        /// <returns>The created BudgetRow.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetRow is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when UserId, BudgetRowId, Name, or Index does not exist or when the BudgetRow with the given id is not found.
        /// </exception>
        Task<BudgetRow> CreateAsync(BudgetRow budgetRow, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a BudgetRow by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetRow to delete.</param>
        /// <returns>True if the operation is successful.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetRow with the given id is not found.
        /// </exception>
        Task DeleteAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Finds a BudgetRow by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetRow to find.</param>
        /// <returns>The found BudgetRow.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetRow with the given id is not found.
        /// </exception>
        Task<BudgetRow> FindByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all BudgetRows.
        /// </summary>
        /// <returns>A list of all BudgetRows.</returns>
        Task<IEnumerable<BudgetRow>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets all BudgetRows for a specific user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of all BudgetRows for the specified user.</returns>
        Task<IEnumerable<BudgetRow>> GetAllAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a BudgetRow.
        /// </summary>
        /// <param name="budgetRow">The BudgetRow to update.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetRow is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when UserId, BudgetRowId, Name, or Index does not exist or when the BudgetRow with the given id is not found.
        /// </exception>
        Task UpdateAsync(BudgetRow budgetRow, CancellationToken cancellationToken);
    }
}