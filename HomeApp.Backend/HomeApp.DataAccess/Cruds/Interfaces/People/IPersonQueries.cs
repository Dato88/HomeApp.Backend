using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Interfaces.People;

public interface IPersonQueries
{
    /// <summary>
    ///     Finds a Person by its id.
    /// </summary>
    /// <param name="id">The id of the Person to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default
    ///     is true.
    /// </param>
    /// <returns>The found Person as a <see cref="Person" />.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Person with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<Person> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Finds a Person by its email address.
    /// </summary>
    /// <param name="email">The email of the Person to find.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default
    ///     is true.
    /// </param>
    /// <returns>The found Person as a <see cref="Person" />.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Person with the given <paramref name="email" /> is not found.
    /// </exception>
    Task<Person> FindByEmailAsync(string email, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all Persons.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default
    ///     is true.
    /// </param>
    /// <returns>A list of all Persons as <see cref="Person" /> objects.</returns>
    Task GetAllAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);
}
