using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Facades;

public class TodoFacade(
    ITodoCommands todoCommands,
    ITodoQueries todoQueries,
    ITodoGroupCrud todoGroupCrud,
    ITodoGroupTodoCrud todoGroupTodoCrud,
    ITodoPersonCrud todoPersonCrud,
    IPersonFacade personFacade,
    ILogger<TodoFacade> logger) : LoggerExtension<TodoFacade>(logger), ITodoFacade
{
    private readonly IPersonFacade _personFacade = personFacade;
    private readonly ITodoCommands _todoCommands = todoCommands;
    private readonly ITodoGroupCrud _todoGroupCrud = todoGroupCrud;
    private readonly ITodoGroupTodoCrud _todoGroupTodoCrud = todoGroupTodoCrud;
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<GetToDoDto> CreateTodoAsync(CreateToDoDto createToDoDto, CancellationToken cancellationToken)
    {
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            createToDoDto.PersonId = person.Id;

            var todo = await _todoCommands.CreateAsync(createToDoDto, cancellationToken);

            if (createToDoDto.TodoGroupId is not null)
            {
                var todoGroupTodo =
                    new TodoGroupTodo { TodoId = todo.Id, TodoGroupId = (int)createToDoDto.TodoGroupId };

                await _todoGroupTodoCrud.CreateAsync(todoGroupTodo, cancellationToken);
            }

            LogInformation($"Creating todo: {todo}", DateTime.Now);

            return todo;
        }
        catch (Exception ex)
        {
            LogError($"Creating todo failed: {ex.Message}", DateTime.Now);

            return null;
        }
    }

    public async Task DeleteTodoAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _todoCommands.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Delete todo failed: {ex}", DateTime.Now);
        }
    }

    public async Task UpdateTodoAsync(UpdateToDoDto updateToDoDto, CancellationToken cancellationToken)
    {
        try
        {
            await _todoCommands.UpdateAsync(updateToDoDto, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Update todo failed: {ex}", DateTime.Now);
        }
    }

    public Task<IEnumerable<GetToDoDto>> GetTodosAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
