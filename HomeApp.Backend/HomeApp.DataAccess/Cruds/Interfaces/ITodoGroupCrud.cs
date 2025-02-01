using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Interfaces;

public interface ITodoGroupCrud
{
    /// <summary>
    ///     Creates a new TodoGroup.
    /// </summary>
    /// <param name="todoGroup">The TodoGroup object to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, returning the created TodoGroup.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="todoGroup" /> is null.
    /// </exception>
    Task<TodoGroup> CreateAsync(TodoGroup todoGroup, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a TodoGroup by its id.
    /// </summary>
    /// <param name="id">The id of the TodoGroup to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous deletion operation. Returns a boolean indicating success.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoGroup with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a TodoGroup by its id.
    /// </summary>
    /// <param name="id">The id of the TodoGroup to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g., related entities).</param>
    /// <returns>The found TodoGroup.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoGroup with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<TodoGroup> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes);

    /// <summary>
    ///     Retrieves all TodoGroups.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g., related entities).</param>
    /// <returns>A list of all TodoGroups.</returns>
    Task<IEnumerable<TodoGroup>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes);

    /// <summary>
    ///     Updates an existing TodoGroup.
    /// </summary>
    /// <param name="todoGroup">The TodoGroup object with updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="todoGroup" /> is null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoGroup with the given <paramref name="todoGroup.Id" /> is not found.
    /// </exception>
    Task UpdateAsync(TodoGroup todoGroup, CancellationToken cancellationToken);
}
