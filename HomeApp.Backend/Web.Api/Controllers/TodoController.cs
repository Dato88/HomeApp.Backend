using Application.Features.Todos.Commands;
using Application.Features.Todos.Queries;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("todos")]
    public async Task<IActionResult> GetTodos(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserTodosQuery(), cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error.Code);
    }

    [HttpGet("todo")]
    public async Task<IActionResult> GetTodo([FromQuery] GetTodoByIdQuery getTodoByIdQuery,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getTodoByIdQuery, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error.Code);
    }

    [HttpPost("todo")]
    public async Task<IActionResult> PostGetToDoDtoAsync([FromBody] CreateTodoCommand createTodoCommand,
        CancellationToken cancellationToken)
    {
        if (createTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(createTodoCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error.Code);
    }

    [HttpDelete("todo")]
    public async Task<IActionResult> DeleteToDoDtoAsync([FromQuery] DeleteTodoCommand deleteTodoCommand,
        CancellationToken cancellationToken)
    {
        if (deleteTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(deleteTodoCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error.Code);
    }

    [HttpPatch("todo")]
    public async Task<IActionResult> UpdateToDoDtoAsync([FromBody] UpdateTodoCommand updateTodoCommand,
        CancellationToken cancellationToken)
    {
        if (updateTodoCommand is null) return BadRequest();

        var todo = await _mediator.Send(updateTodoCommand, cancellationToken);

        return Ok(todo);
    }
}
