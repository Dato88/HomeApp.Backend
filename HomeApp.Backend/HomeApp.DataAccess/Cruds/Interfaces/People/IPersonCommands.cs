using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Interfaces.People;

public interface IPersonCommands
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
    Task<Person> CreateAsync(Person person, CancellationToken cancellationToken);

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
    Task<Person> UpdateAsync(Person person, CancellationToken cancellationToken);
}
