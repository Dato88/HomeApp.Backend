using HomeApp.Library.Models.Data_Transfer_Objects.TodoDtos;

namespace HomeApp.Library.Facades.Interfaces;

public interface ITodoFacade
{
    /// <summary>
    ///     Creates a new Todo.
    /// </summary>
    /// <param name="createToDoDto">
    ///     The DTO containing the details of the Todo to create.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. Returns the created Todo as a DTO.
    /// </returns>
    Task<GetToDoDto> CreateTodoAsync(CreateToDoDto createToDoDto, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a Todo by its ID.
    /// </summary>
    /// <param name="id">The ID of the Todo to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous operation.
    /// </returns>
    Task DeleteTodoAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing Todo.
    /// </summary>
    /// <param name="updateToDoDto">
    ///     The DTO containing the updated details of the Todo.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. Returns the updated Todo as a DTO.
    /// </returns>
    Task<GetToDoDto> UpdateTodoAsync(UpdateToDoDto updateToDoDto, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves all Todos.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. Returns a collection of Todos as DTOs.
    /// </returns>
    Task<IEnumerable<GetToDoDto>> GetTodosAsync(CancellationToken cancellationToken);
}
