namespace HomeApp.Library.Cruds.Interfaces
{
    public interface IBudgetGroupCrud
    {
        /// <summary>
        /// Creates a new BudgetGroup.
        /// </summary>
        /// <param name="budgetGroup">The BudgetGroup to create.</param>
        /// <returns>The created BudgetGroup.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetGroup is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when UserId, BudgetGroupId, Name, or Index does not exist or when the BudgetGroup with the given id is not found.
        /// </exception>
        Task<BudgetGroup> CreateAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a BudgetGroup by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetGroup to delete.</param>
        /// <returns>True if the operation is successful.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetGroup with the given id is not found.
        /// </exception>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Finds a BudgetGroup by its id.
        /// </summary>
        /// <param name="id">The id of the BudgetGroup to find.</param>
        /// <returns>The found BudgetGroup.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the BudgetGroup with the given id is not found.
        /// </exception>
        Task<BudgetGroup> FindByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all BudgetGroups.
        /// </summary>
        /// <returns>A list of all BudgetGroups.</returns>
        Task<IEnumerable<BudgetGroup>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets all BudgetGroups for a specific user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of all BudgetGroups for the specified user.</returns>
        Task<IEnumerable<BudgetGroup>> GetAllAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a BudgetGroup.
        /// </summary>
        /// <param name="budgetGroup">The BudgetGroup to update.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when budgetGroup is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when UserId, BudgetGroupId, Name, or Index does not exist or when the BudgetGroup with the given id is not found.
        /// </exception>
        Task UpdateAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);
    }
}