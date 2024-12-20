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

    [HttpGet("todos")]
    public async Task<IEnumerable<GetToDoDto>> GetTodos(CancellationToken cancellationToken)
    {
        var todos = await _todoFacade.GetTodosAsync(cancellationToken);

        return todos;
    }

    [HttpPost("todo")]
    public async Task<GetToDoDto> PostGetToDoDtoAsync([FromBody] CreateToDoDto createToDoDto,
        CancellationToken cancellationToken)
    {
        var todo = await _todoFacade.CreateTodoAsync(createToDoDto, cancellationToken);

        return todo;
    }
}
