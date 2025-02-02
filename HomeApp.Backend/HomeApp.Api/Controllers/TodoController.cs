﻿using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Todos.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TodoController(IMediator mediator, ITodoFacade todoFacade) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ITodoFacade _todoFacade = todoFacade;

    [HttpGet("todos")]
    public async Task<IActionResult> GetTodos(CancellationToken cancellationToken)
    {
        var todos = await _mediator.Send(new GetUserTodosQuery(), cancellationToken);

        return Ok(todos);
    }

    [HttpPost("todo")]
    public async Task<IActionResult> PostGetToDoDtoAsync([FromBody] CreateToDoDto createToDoDto,
        CancellationToken cancellationToken)
    {
        var todo = await _todoFacade.CreateTodoAsync(createToDoDto, cancellationToken);

        return Ok(todo);
    }

    [HttpDelete("todo")]
    public async Task<IActionResult> DeleteToDoDtoAsync([FromQuery] int id,
        CancellationToken cancellationToken)
    {
        await _todoFacade.DeleteTodoAsync(id, cancellationToken);

        return Ok();
    }

    [HttpPatch("todo")]
    public async Task<IActionResult> UpdateToDoDtoAsync([FromBody] UpdateToDoDto updateToDoDto,
        CancellationToken cancellationToken)
    {
        await _todoFacade.UpdateTodoAsync(updateToDoDto, cancellationToken);

        return Ok();
    }
}
