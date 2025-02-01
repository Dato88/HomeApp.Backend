using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Interfaces;

public interface ITodoGroupTodoCrud
{
    /// <summary>
    ///     Creates a new TodoGroupTodo.
    /// </summary>
    /// <param name="todoGroupTodo">The TodoGroupTodo object to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, returning the created TodoGroupTodo.</returns>
    Task<TodoGroupTodo> CreateAsync(TodoGroupTodo todoGroupTodo,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a TodoGroupTodo by its id.
    /// </summary>
    /// <param name="id">The id of the TodoGroupTodo to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous deletion operation. Returns a boolean indicating success.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoGroupTodo with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a TodoGroupTodo by its id.
    /// </summary>
    /// <param name="id">The id of the TodoGroupTodo to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g., related entities).</param>
    /// <returns>The found TodoGroupTodo as a <see cref="TodoGroupTodo" />.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoGroupTodo with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<TodoGroupTodo> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes);

    /// <summary>
    ///     Retrieves all TodoGroupTodos.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g., related entities).</param>
    /// <returns>A list of all TodoGroupTodos.</returns>
    Task<IEnumerable<TodoGroupTodo>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes);

    /// <summary>
    ///     Updates an existing TodoGroupTodo.
    /// </summary>
    /// <param name="todoGroupTodo">The TodoGroupTodo object with updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoGroupTodo with the given <paramref name="todoGroupTodo.Id" /> is not found.
    /// </exception>
    Task UpdateAsync(TodoGroupTodo todoGroupTodo, CancellationToken cancellationToken);
}
