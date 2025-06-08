using Application.Features.Todos.Commands;
using Application.Features.Todos.Queries;
using Web.Api.Requests.Todo;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("todos")]
    public async Task<IActionResult> GetTodosAsync(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserTodosQuery(), cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("todo")]
    public async Task<IActionResult> GetTodoAsync([FromQuery] GetTodoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((GetTodoByIdQuery)request, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("todo")]
    public async Task<IActionResult> CreateToDoAsync([FromBody] CreateTodoRequest? createTodoRequest,
        CancellationToken cancellationToken)
    {
        if (createTodoRequest is null) return BadRequest();

        var response = await _mediator.Send((CreateTodoCommand)createTodoRequest, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpDelete("todo")]
    public async Task<IActionResult> DeleteToDoAsync([FromQuery] DeleteTodoRequest? deleteTodoRequest,
        CancellationToken cancellationToken)
    {
        if (deleteTodoRequest is null) return BadRequest();

        var response = await _mediator.Send((DeleteTodoCommand)deleteTodoRequest, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpPatch("todo")]
    public async Task<IActionResult> UpdateToDoAsync([FromBody] UpdateTodoRequest? updateTodoRequest,
        CancellationToken cancellationToken)
    {
        if (updateTodoRequest is null) return BadRequest();

        var response = await _mediator.Send((UpdateTodoCommand)updateTodoRequest, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }
}
