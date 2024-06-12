namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetColumnCrud
    {
        /// <summary>
        /// Creates a new BudgetColumn.
        /// </summary>
        /// <param name="budgetColumn">The BudgetColumn to create.</param>
        /// <returns>The created BudgetColumn.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetColumn is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when BudgetColumnId, Name, or Index does not exist or when the BudgetColumn with the given id is not found.
        /// </exception>
        Task<BudgetColumn> CreateAsync(BudgetColumn budgetColumn);

        /// <summary>
        /// Deletes a BudgetColumn by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetColumn to delete.</param>
        /// <returns>True if the operation is successful.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetColumn with the given id is not found.
        /// </exception>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Finds a BudgetColumn by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetColumn to find.</param>
        /// <returns>The found BudgetColumn.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetColumn with the given id is not found.
        /// </exception>
        Task<BudgetColumn> FindByIdAsync(int id);

        /// <summary>
        /// Gets all BudgetColumns.
        /// </summary>
        /// <returns>A list of all BudgetColumns.</returns>
        Task<IEnumerable<BudgetColumn>> GetAllAsync();

        /// <summary>
        /// Updates a BudgetColumn.
        /// </summary>
        /// <param name="budgetColumn">The BudgetColumn to update.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetColumn is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when BudgetColumnId, Name, or Index does not exist or when the BudgetColumn with the given id is not found.
        /// </exception>
        Task UpdateAsync(BudgetColumn budgetColumn);

    }
}