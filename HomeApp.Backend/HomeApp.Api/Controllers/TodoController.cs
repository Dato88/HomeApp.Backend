﻿using HomeApp.Library.Todos.Commands;
using HomeApp.Library.Todos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

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

        if (response.Succcess) return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("todo")]
    public async Task<IActionResult> PostGetToDoDtoAsync([FromBody] CreateTodoCommand createTodoCommand,
        CancellationToken cancellationToken)
    {
        if (createTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(createTodoCommand, cancellationToken);

        if (response.Succcess) return Ok(response);

        return BadRequest(response);
    }

    [HttpDelete("todo")]
    public async Task<IActionResult> DeleteToDoDtoAsync([FromQuery] DeleteTodoCommand deleteTodoCommand,
        CancellationToken cancellationToken)
    {
        if (deleteTodoCommand is null) return BadRequest();

        var response = await _mediator.Send(deleteTodoCommand, cancellationToken);

        if (response.Succcess) return Ok(response);

        return BadRequest(response);
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
