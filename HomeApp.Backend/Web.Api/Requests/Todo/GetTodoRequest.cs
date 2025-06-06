using Application.Features.Todos.Queries;

namespace Web.Api.Requests.Todo;

public sealed record GetTodoRequest(int Id)
{
    public static explicit operator GetTodoByIdQuery(GetTodoRequest request)
        => new(request.Id);
}
