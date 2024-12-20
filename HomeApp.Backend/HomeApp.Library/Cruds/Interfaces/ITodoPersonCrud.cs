namespace HomeApp.Library.Cruds.Interfaces;

public interface ITodoPersonCrud
{
    /// <summary>
    ///     Creates a new TodoPerson.
    /// </summary>
    /// <param name="todoPerson">The TodoPerson object to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, returning the created TodoPerson.</returns>
    Task<TodoPerson> CreateAsync(TodoPerson todoPerson, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a TodoPerson by its id.
    /// </summary>
    /// <param name="id">The id of the TodoPerson to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous deletion operation. Returns a boolean indicating success.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoPerson with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds a TodoPerson by its id.
    /// </summary>
    /// <param name="id">The id of the TodoPerson to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g., related entities).</param>
    /// <returns>The found TodoPerson as a <see cref="TodoPerson" />.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoPerson with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<TodoPerson> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes);

    /// <summary>
    ///     Retrieves all TodoPersons.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g., related entities).</param>
    /// <returns>A list of all TodoPersons.</returns>
    Task<IEnumerable<TodoPerson>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes);

    /// <summary>
    ///     Updates an existing TodoPerson.
    /// </summary>
    /// <param name="todoPerson">The TodoPerson object with updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the TodoPerson with the given <paramref name="todoPerson.Id" /> is not found.
    /// </exception>
    Task UpdateAsync(TodoPerson todoPerson, CancellationToken cancellationToken);
}
