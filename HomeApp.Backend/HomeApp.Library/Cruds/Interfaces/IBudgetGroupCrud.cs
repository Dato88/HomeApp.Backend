namespace HomeApp.Library.Cruds.Interfaces;

public interface IBudgetGroupCrud
{
    /// <summary>
    ///     Creates a new BudgetGroup.
    /// </summary>
    /// <param name="budgetGroup">The BudgetGroup to create.</param>
    /// <returns>The created BudgetGroup.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="budgetGroup" /> is null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when any of the following are invalid:
    ///     - <paramref name="budgetGroup.UserId" />
    ///     - <paramref name="budgetGroup.BudgetGroupId" />
    ///     - <paramref name="budgetGroup.Name" />
    ///     - <paramref name="budgetGroup.Index" />
    /// </exception>
    Task<BudgetGroup> CreateAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a BudgetGroup by its id.
    /// </summary>
    /// <param name="id">The id of the BudgetGroup to delete.</param>
    /// <returns>True if the operation is successful, otherwise false.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the BudgetGroup with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a BudgetGroup by its id.
    /// </summary>
    /// <param name="id">The id of the BudgetGroup to find.</param>
    /// <returns>The found BudgetGroup.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the BudgetGroup with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<BudgetGroup> FindByIdAsync(int id, CancellationToken cancellationToken,
        params string[] includes);

    /// <summary>
    ///     Gets all BudgetGroups.
    /// </summary>
    /// <returns>A list of all BudgetGroups.</returns>
    Task<IEnumerable<BudgetGroup>> GetAllAsync(CancellationToken cancellationToken,
        params string[] includes);

    /// <summary>
    ///     Gets all BudgetGroups for a specific user.
    /// </summary>
    /// <param name="userId">The id of the user.</param>
    /// <returns>A list of all BudgetGroups associated with the specified user.</returns>
    Task<IEnumerable<BudgetGroup>> GetAllAsync(int userId, CancellationToken cancellationToken,
        params string[] includes);

    /// <summary>
    ///     Updates an existing BudgetGroup.
    /// </summary>
    /// <param name="budgetGroup">The BudgetGroup to update.</param>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="budgetGroup" /> is null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when any of the following are invalid:
    ///     - <paramref name="budgetGroup.UserId" />
    ///     - <paramref name="budgetGroup.BudgetGroupId" />
    ///     - <paramref name="budgetGroup.Name" />
    ///     - <paramref name="budgetGroup.Index" />
    ///     - When the BudgetGroup with the given <paramref name="budgetGroup.BudgetGroupId" /> does not exist.
    /// </exception>
    Task UpdateAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);
}
