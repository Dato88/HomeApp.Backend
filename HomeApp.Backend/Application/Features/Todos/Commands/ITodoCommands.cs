using Domain.Entities.Todos;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Commands;

public interface ITodoCommands
{
    /// <summary>
    ///     Creates a new Todo item asynchronously.
    /// </summary>
    /// <param name="todo">The Todo item to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Result{TValue}" /> containing the ID of the created Todo item on success,
    ///     or an error describing the failure.
    /// </returns>
    Task<Result<int>> CreateAsync(Todo todo, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a Todo item by its ID asynchronously.
    /// </summary>
    /// <param name="todoId">The ID of the Todo item to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Result" /> indicating success or containing an error if the deletion failed.
    /// </returns>
    Task<Result> DeleteAsync(TodoId todoId, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing Todo item asynchronously.
    /// </summary>
    /// <param name="todo">The Todo item containing updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Result" /> indicating success or containing an error if the update failed.
    /// </returns>
    Task<Result> UpdateAsync(Todo todo, CancellationToken cancellationToken);
}
