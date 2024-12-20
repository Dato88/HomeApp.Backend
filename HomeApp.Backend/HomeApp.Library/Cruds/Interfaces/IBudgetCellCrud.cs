namespace HomeApp.Library.Cruds.Interfaces;

public interface IBudgetCellCrud
{
    /// <summary>
    ///     Creates a new BudgetCell.
    /// </summary>
    /// <param name="budgetCell">The BudgetCell to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created BudgetCell.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="budgetCell" /> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when UserId, BudgetRowId, BudgetColumnId, or BudgetGroupId do not exist.
    /// </exception>
    Task<BudgetCell> CreateAsync(BudgetCell budgetCell, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a BudgetCell by its ID.
    /// </summary>
    /// <param name="id">The ID of the BudgetCell to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the deletion was successful, otherwise False.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the BudgetCell with the given ID cannot be found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a BudgetCell by its ID.
    /// </summary>
    /// <param name="id">The ID of the BudgetCell to be found.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entity should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>The found BudgetCell.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the BudgetCell with the given ID cannot be found.
    /// </exception>
    Task<BudgetCell> FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all BudgetCells.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entities should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>A list of all BudgetCells.</returns>
    Task<IEnumerable<BudgetCell>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all BudgetCells for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entities should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>A list of all BudgetCells for the specified user.</returns>
    Task<IEnumerable<BudgetCell>> GetAllAsync(int userId, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Updates an existing BudgetCell.
    /// </summary>
    /// <param name="budgetCell">The BudgetCell to be updated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="budgetCell" /> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when UserId, BudgetRowId, BudgetColumnId, or BudgetGroupId do not exist or when the
    ///     BudgetCell with the specified ID cannot be found.
    /// </exception>
    Task UpdateAsync(BudgetCell budgetCell, CancellationToken cancellationToken);
}
