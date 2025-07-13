using Application.Features.Todos.Queries;

namespace Web.Api.Requests.Todo;

public sealed record GetTodoRequest(int TodoId)
{
    public static explicit operator GetTodoByIdQuery(GetTodoRequest request)
        => new(request.TodoId);
}
