using SharedKernel;

namespace Domain.Entities.Budgets;

public static class BudgetErrors
{
    public static readonly Error NotFoundAll = Error.NotFound(
        "Budget.NotFoundAll",
        "There are no budgets in the database");

    public static readonly Error NotFound = Error.NotFound(
        "Budget.NotFound",
        "There is no budget in the database");

    public static Error UnexpectedError(string message) => Error.Failure(
        "Unexpected error",
        $"An unexpected error occurred: {message}"
    );

    public static Error NotFoundById(int budgetId) => Error.NotFound(
        "Budget.NotFoundById",
        $"The budget with the Id = '{budgetId}' was not found");

    public static Error CreateFailedWithMessage(string message) => Error.Failure(
        "Budget.CreateFailedWithMessage",
        $"The budget could not be created with message = '{message}'");

    public static Error DeleteFailed(int budgetId) => Error.Failure(
        "Budget.DeleteFailed",
        $"The budget with the id = '{budgetId}' could not be deleted");

    public static Error UpdateFailedWithMessage(string message) => Error.Failure(
        "Budget.UpdateFailedWithMessage",
        $"The budget could not be updated with message = '{message}'");
}
