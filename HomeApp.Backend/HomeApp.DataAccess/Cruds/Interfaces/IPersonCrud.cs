using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.PersonDtos;

namespace HomeApp.DataAccess.Cruds.Interfaces;

public interface IPersonCrud
{
    /// <summary>
    ///     Creates a new Person.
    /// </summary>
    /// <param name="person">The Person object to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="person" /> is null.
    /// </exception>
    Task CreateAsync(Person person, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a Person by its user id.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous deletion operation. Returns a boolean indicating success.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Person with the given <paramref name="id" /> is not found.
    /// </exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);

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
    /// <returns>The found Person as a <see cref="PersonDto" />.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Person with the given <paramref name="id" /> is not found.
    /// </exception>
    Task<PersonDto> FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
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
    /// <returns>The found Person as a <see cref="PersonDto" />.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Person with the given <paramref name="email" /> is not found.
    /// </exception>
    Task<PersonDto> FindByEmailAsync(string email, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all Persons.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default
    ///     is true.
    /// </param>
    /// <returns>A list of all Persons as <see cref="PersonDto" /> objects.</returns>
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Updates an existing Person.
    /// </summary>
    /// <param name="person">The Person object with updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when the <paramref name="person" /> is null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the Person with the given <paramref name="person.Id" /> is not found.
    /// </exception>
    Task UpdateAsync(Person person, CancellationToken cancellationToken);
}
