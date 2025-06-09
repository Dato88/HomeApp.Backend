using Application.Features.Todos.Commands;

namespace Web.Api.Requests.Todo;

public sealed record DeleteTodoRequest(int Id)
{
    public static explicit operator DeleteTodoCommand(DeleteTodoRequest request)
        => new(request.Id);
}
