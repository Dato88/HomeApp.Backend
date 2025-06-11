using Domain.Entities.Todos;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Queries;

public interface ITodoQueries
{
    /// <summary>
    ///     Finds a Todo by its id.
    /// </summary>
    /// <param name="todoId">The id of the Todo to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>
    ///     A <see cref="Result{Todo}" /> representing the operation result. Contains the found Todo or an error if not found.
    /// </returns>
    Task<Result<Todo>> FindByIdAsync(TodoId todoId, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all Todos for a specific Person.
    /// </summary>
    /// <param name="personId">The id of the Person whose Todos should be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>
    ///     A <see cref="Result{IEnumerable{Todo}}" /> containing the list of Todos or an error if none are found.
    /// </returns>
    Task<Result<IEnumerable<Todo>>> GetAllAsync(PersonId personId, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes);
}
