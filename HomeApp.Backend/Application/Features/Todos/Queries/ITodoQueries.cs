using Domain.Entities.Todos;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public interface ITodoQueries
{
    /// <summary>
    ///     Finds a Todo by its id.
    /// </summary>
    /// <param name="todoId">The id of the Todo to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Result{Todo}" /> representing the operation result. Contains the found Todo or an error if not found.
    /// </returns>
    Task<Result<Todo>> FindTodoByIdAsync(int todoId, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves all Todos for a specific Person.
    /// </summary>
    /// <param name="personId">The id of the Person whose Todos should be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// /// <returns>
    ///     A <see cref="Result{IEnumerable{Todo}}" /> containing the list of Todos or an error if none are found.
    /// </returns>
    Task<Result<IEnumerable<Todo>>> GetAllUserTodosAsync(int personId, CancellationToken cancellationToken);
}
