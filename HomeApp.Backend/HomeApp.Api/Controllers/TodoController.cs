using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models.Data_Transfer_Objects.TodoDtos;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TodoController(ITodoFacade todoFacade) : ControllerBase
{
    private readonly ITodoFacade _todoFacade = todoFacade;

    [HttpGet("todo")]
    public async Task<IEnumerable<GetToDoDto>> GetTodo(CancellationToken cancellationToken)
    {
        var todos = await _todoFacade.GetTodosAsync(cancellationToken);

        return todos;
    }

    [HttpPost("todo")]
    public async Task<GetToDoDto> PostBudgetCellAsync([FromBody] CreateToDoDto createToDoDto,
        CancellationToken cancellationToken)
    {
        var todo = await _todoFacade.CreateTodoAsync(createToDoDto, cancellationToken);

        return todo;
    }
}
