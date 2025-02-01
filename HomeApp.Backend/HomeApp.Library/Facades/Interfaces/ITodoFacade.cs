using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;

namespace HomeApp.Library.Facades.Interfaces;

public interface ITodoFacade
{
    Task<GetToDoDto> CreateTodoAsync(CreateToDoDto createToDoDto, CancellationToken cancellationToken);
    Task DeleteTodoAsync(int id, CancellationToken cancellationToken);
    Task UpdateTodoAsync(UpdateToDoDto updateToDoDto, CancellationToken cancellationToken);
    Task<IEnumerable<GetToDoDto>> GetTodosAsync(CancellationToken cancellationToken);
}
