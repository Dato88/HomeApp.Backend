﻿using Application.Features.Todos.Commands;
using Application.Features.Todos.Queries;

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
    public async Task<IActionResult> GetTodoAsync([FromQuery] GetTodoByIdQuery getTodoByIdQuery,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getTodoByIdQuery, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("todo")]
    public async Task<IActionResult> CreateToDoAsync([FromBody] CreateTodoCommand createTodoCommand,
        CancellationToken cancellationToken)
    {
        if (createTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(createTodoCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpDelete("todo")]
    public async Task<IActionResult> DeleteToDoAsync([FromQuery] DeleteTodoCommand deleteTodoCommand,
        CancellationToken cancellationToken)
    {
        if (deleteTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(deleteTodoCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }

    [HttpPatch("todo")]
    public async Task<IActionResult> UpdateToDoAsync([FromBody] UpdateTodoCommand updateTodoCommand,
        CancellationToken cancellationToken)
    {
        if (updateTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(updateTodoCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);
    }
}
