using Domain.Todos;

namespace Application.Common.Interfaces.Todos;

public interface ITodoCommands
{
    /// <summary>
    ///     Creates a new Todo item asynchronously.
    /// </summary>
    /// <param name="todo">The Todo item to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, returning the ID of the created Todo item.</returns>
    Task<int> CreateAsync(Todo todo, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a Todo item by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the Todo item to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous deletion operation. Returns <c>true</c> if the item was deleted
    ///     successfully; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown if the Todo item with the specified <paramref name="id" /> does not exist.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing Todo item asynchronously.
    /// </summary>
    /// <param name="todo">The Todo item containing updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous update operation. Returns <c>true</c> if the update was successful;
    ///     otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown if the Todo item with the specified <paramref name="todo.Id" /> does not exist.
    /// </exception>
    Task<bool> UpdateAsync(Todo todo, CancellationToken cancellationToken);
}
