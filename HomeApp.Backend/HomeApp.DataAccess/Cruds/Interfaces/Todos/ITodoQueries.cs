using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;

namespace HomeApp.DataAccess.Cruds.Interfaces.Todos;

public interface ITodoQueries
{
    /// <summary>
    ///     Finds a Todo by its id.
    /// </summary>
    /// <param name="id">The id of the Todo to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default
    ///     is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>The found Todo object.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Todo with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<Todo> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all Todos by Person id.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of all TodoPersons.</returns>
    Task<IEnumerable<GetToDoDto>> GetAllAsync(int personId, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes);
}
