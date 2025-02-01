using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Facades;

public class TodoFacade(
    ITodoCrud todoCrud,
    ITodoGroupCrud todoGroupCrud,
    ITodoGroupTodoCrud todoGroupTodoCrud,
    ITodoPersonCrud todoPersonCrud,
    IPersonFacade personFacade,
    ILogger<TodoFacade> logger) : LoggerExtension<TodoFacade>(logger), ITodoFacade
{
    private readonly IPersonFacade _personFacade = personFacade;
    private readonly ITodoCrud _todoCrud = todoCrud;
    private readonly ITodoGroupCrud _todoGroupCrud = todoGroupCrud;
    private readonly ITodoGroupTodoCrud _todoGroupTodoCrud = todoGroupTodoCrud;
    private readonly ITodoPersonCrud _todoPersonCrud = todoPersonCrud;

    public async Task<GetToDoDto> CreateTodoAsync(CreateToDoDto createToDoDto, CancellationToken cancellationToken)
    {
        try
        {
            var todo = await _todoCrud.CreateAsync(createToDoDto, cancellationToken);

            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            var todoPerson = new CreateToDoPersonDto(person.Id, todo.Id);

            await _todoPersonCrud.CreateAsync(todoPerson, cancellationToken);

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
            await _todoCrud.DeleteAsync(id, cancellationToken);
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
            await _todoCrud.UpdateAsync(updateToDoDto, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Update todo failed: {ex}", DateTime.Now);
        }
    }

    public async Task<IEnumerable<GetToDoDto>> GetTodosAsync(CancellationToken cancellationToken)
    {
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            return await _todoCrud.GetAllAsync(person.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Get todo failed: {ex}", DateTime.Now);

            return null;
        }
    }
}
