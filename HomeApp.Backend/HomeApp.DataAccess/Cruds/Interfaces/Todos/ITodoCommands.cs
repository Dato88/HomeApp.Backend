using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Interfaces.Todos;

public interface ITodoCommands
{
    /// <summary>
    ///     Creates a new Todo.
    /// </summary>
    /// <param name="todo">The Todo object to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with the created Todo object as the result.</returns>
    Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a Todo by its id.
    /// </summary>
    /// <param name="id">The id of the Todo to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous deletion operation. Returns a boolean indicating success.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Todo with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing Todo.
    /// </summary>
    /// <param name="todo">The Todo object with updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Todo with the given <paramref name="todo.Id" /> is not found.
    /// </exception>
    Task UpdateAsync(Todo todo, CancellationToken cancellationToken);
}
