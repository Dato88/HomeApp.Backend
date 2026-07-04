using Application.Features.Budgets.Commands.Update;

namespace Web.Api.Requests.Budget;

public sealed record UpdateBudgetCellRequest
{
    public int BudgetCellId { get; init; }
    public decimal Amount { get; init; }

    public static explicit operator UpdateBudgetCellCommand(UpdateBudgetCellRequest request)
        => new(request.BudgetCellId, request.Amount);
}
