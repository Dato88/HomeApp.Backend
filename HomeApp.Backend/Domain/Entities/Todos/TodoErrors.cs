using SharedKernel;

namespace Domain.Entities.Todos;

public static class TodoErrors
{
    public static readonly Error CreateFailed = Error.Failure(
        "Todo.CreateFailed",
        "The todo could not be created");

    public static readonly Error NotFoundAll = Error.NotFound(
        "Todo.NotFoundAll",
        "There are no todos available");

    public static Error NotFoundById(int id) => Error.NotFound(
        "Todo.NotFoundById",
        $"The todo with the Id = '{id}' was not found");

    public static Error NotFoundWithMessage(string message) => Error.NotFound(
        "Todo.NotFoundWithMessage",
        $"The todo could not be found: {message}");

    public static Error CreateFailedWithMessage(string message) => Error.Failure(
        "Todo.CreateFailedWithMessage",
        $"The todo could not be created with message = '{message}'");

    public static Error DeleteFailedWithMessage(string message) => Error.Failure(
        "Todo.DeleteFailedWithMessage",
        $"The todo could not be deleted with message = '{message}'");

    public static Error DeleteFailed(int id) => Error.Failure(
        "Todo.DeleteFailed",
        $"The todo with the id = '{id}' could not be deleted");

    public static Error UpdateFailedWithMessage(string message) => Error.Failure(
        "Todo.UpdateFailedWithMessage",
        $"The todo could not be updated with message = '{message}'");

    public static Error UpdateFailed(int id) => Error.Failure(
        "Todo.UpdateFailed",
        $"The todo with the id = '{id}' could not be updated");
}
