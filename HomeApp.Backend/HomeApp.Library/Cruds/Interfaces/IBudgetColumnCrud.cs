namespace HomeApp.Library.Cruds.Interfaces;

public interface IBudgetColumnCrud
{
    /// <summary>
    ///     Creates a new BudgetColumn.
    /// </summary>
    /// <param name="budgetColumn">The BudgetColumn to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created BudgetColumn.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="budgetColumn" /> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when BudgetColumnId, Name, or Index do not exist or when the BudgetColumn with the given
    ///     ID cannot be found.
    /// </exception>
    Task<BudgetColumn> CreateAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a BudgetColumn by its ID.
    /// </summary>
    /// <param name="id">The ID of the BudgetColumn to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the deletion was successful, otherwise False.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the BudgetColumn with the given ID cannot be found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a BudgetColumn by its ID.
    /// </summary>
    /// <param name="id">The ID of the BudgetColumn to be found.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entity should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>The found BudgetColumn.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the BudgetColumn with the given ID cannot be found.
    /// </exception>
    Task<BudgetColumn> FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all BudgetColumns.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entities should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>A list of all BudgetColumns.</returns>
    Task<IEnumerable<BudgetColumn>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Updates an existing BudgetColumn.
    /// </summary>
    /// <param name="budgetColumn">The BudgetColumn to be updated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="budgetColumn" /> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when BudgetColumnId, Name, or Index do not exist or when the BudgetColumn with the given
    ///     ID cannot be found.
    /// </exception>
    Task UpdateAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken);
}
