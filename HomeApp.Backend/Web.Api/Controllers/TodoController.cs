using Application.Features.Todos.Commands;
using Application.Features.Todos.Dtos;
using Application.Features.Todos.Queries;
using SharedKernel;
using Web.Api.Requests.Todo;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("todos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<IEnumerable<GetToDoResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetTodosAsync(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserTodosQuery(), cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("todo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GetToDoResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetTodoAsync([FromQuery] int todoId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetTodoByIdQuery(todoId), cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("todo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> CreateToDoAsync([FromBody] CreateTodoRequest? createTodoRequest,
        CancellationToken cancellationToken)
    {
        if (createTodoRequest is null) return BadRequest();

        var response = await _mediator.Send((CreateTodoCommand)createTodoRequest, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("todo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteToDoAsync([FromQuery] int todoId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteTodoCommand(todoId), cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("todo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateToDoAsync([FromBody] UpdateTodoRequest? updateTodoRequest,
        CancellationToken cancellationToken)
    {
        if (updateTodoRequest is null) return BadRequest();

        var response = await _mediator.Send((UpdateTodoCommand)updateTodoRequest, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
