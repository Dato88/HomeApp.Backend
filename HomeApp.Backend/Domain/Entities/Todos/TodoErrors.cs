using SharedKernel;
using SharedKernel.ValueObjects;

namespace Domain.Entities.Todos;

public static class TodoErrors
{
    public static readonly Error CreateFailed = Error.Failure(
        "Todo.CreateFailed",
        "The todo could not be created");

    public static readonly Error NotFoundAll = Error.NotFound(
        "Todo.NotFoundAll",
        "There are no todos available");

    public static Error NotFoundById(TodoId id) => Error.NotFound(
        "Todo.NotFoundById",
        $"The todo with the Id = '{id}' was not found");

    public static Error NotFoundWithMessage(string message) => Error.NotFound(
        "Todo.NotFoundWithMessage",
        $"The todo could not be found: {message}");

    public static Error CreateFailedWithMessage(string message) => Error.Failure(
        "Todo.CreateFailedWithMessage",
        $"The todo could not be created with message = '{message}'");

    public static Error DeleteFailed(TodoId todoId) => Error.Failure(
        "Todo.DeleteFailed",
        $"The todo with the id = '{todoId}' could not be deleted");

    public static Error UpdateFailedWithMessage(string message) => Error.Failure(
        "Todo.UpdateFailedWithMessage",
        $"The todo could not be updated with message = '{message}'");

    public static Error UpdateFailed(TodoId id) => Error.Failure(
        "Todo.UpdateFailed",
        $"The todo with the id = '{id}' could not be updated");
}
