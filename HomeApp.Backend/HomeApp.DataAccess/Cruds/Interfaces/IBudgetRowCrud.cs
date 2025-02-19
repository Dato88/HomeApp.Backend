using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Interfaces;

public interface IBudgetRowCrud
{
    /// <summary>
    ///     Creates a new BudgetRow.
    /// </summary>
    /// <param name="budgetRow">The BudgetRow to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created BudgetRow.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="budgetRow" /> is null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when any of the following are invalid:
    ///     - <paramref name="budgetRow.UserId" />
    ///     - <paramref name="budgetRow.BudgetRowId" />
    ///     - <paramref name="budgetRow.Name" />
    ///     - <paramref name="budgetRow.Index" />
    /// </exception>
    Task<BudgetRow> CreateAsync(BudgetRow budgetRow, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a BudgetRow by its id.
    /// </summary>
    /// <param name="id">The id of the BudgetRow to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the deletion operation.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the BudgetRow with the given <paramref name="id" /> is not found.
    /// </exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a BudgetRow by its id.
    /// </summary>
    /// <param name="id">The id of the BudgetRow to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entity should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>The found BudgetRow.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the BudgetRow with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<BudgetRow> FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Gets all BudgetRows.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entities should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>A list of all BudgetRows.</returns>
    Task<IEnumerable<BudgetRow>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Gets all BudgetRows for a specific user.
    /// </summary>
    /// <param name="userId">The id of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag indicating whether the entities should be loaded with <c>AsNoTracking()</c>.
    ///     Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>A list of all BudgetRows associated with the specified user.</returns>
    Task<IEnumerable<BudgetRow>> GetAllAsync(int userId, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Updates an existing BudgetRow.
    /// </summary>
    /// <param name="budgetRow">The BudgetRow to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="budgetRow" /> is null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when any of the following are invalid:
    ///     - <paramref name="budgetRow.UserId" />
    ///     - <paramref name="budgetRow.BudgetRowId" />
    ///     - <paramref name="budgetRow.Name" />
    ///     - <paramref name="budgetRow.Index" />
    ///     - When the BudgetRow with the given <paramref name="budgetRow.BudgetRowId" /> does not exist.
    /// </exception>
    Task UpdateAsync(BudgetRow budgetRow, CancellationToken cancellationToken);
}
