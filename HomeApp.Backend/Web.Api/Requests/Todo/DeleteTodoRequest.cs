using Application.Features.Todos.Commands;
using SharedKernel.ValueObjects;

namespace Web.Api.Requests.Todo;

public sealed record DeleteTodoRequest(TodoId TodoId)
{
    public static explicit operator DeleteTodoCommand(DeleteTodoRequest request)
        => new(request.TodoId);
}
