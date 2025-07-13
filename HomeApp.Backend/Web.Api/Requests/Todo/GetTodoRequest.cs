using Application.Features.Todos.Queries;
using Domain.ValueObjects;

namespace Web.Api.Requests.Todo;

public sealed record GetTodoRequest(TodoId TodoId)
{
    public static explicit operator GetTodoByIdQuery(GetTodoRequest request)
        => new(request.TodoId);
}
