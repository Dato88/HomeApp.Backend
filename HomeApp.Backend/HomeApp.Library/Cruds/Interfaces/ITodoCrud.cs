﻿using HomeApp.Library.Models.Data_Transfer_Objects.TodoDtos;

namespace HomeApp.Library.Cruds.Interfaces;

public interface ITodoCrud
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
    Task<Todo> FindByIdAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all Todos.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asNoTracking">
    ///     Optional flag to enable or disable the use of <c>AsNoTracking()</c> for the query. Default
    ///     is true.
    /// </param>
    /// <param name="includes">Optional additional properties to include in the result (e.g. related entities).</param>
    /// <returns>A list of all Todos.</returns>
    Task GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes);

    /// <summary>
    ///     Retrieves all Todos by Person id.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of all TodoPersons.</returns>
    Task<IEnumerable<GetToDoDto>> GetAllAsync(int personId, CancellationToken cancellationToken);

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
