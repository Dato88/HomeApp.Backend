using Application.Features.Budgets.Commands;

namespace Web.Api.Requests.Budget;

public record CreateBudgetCellRequest
{
    public int BudgetRowId { get; init; }
    public int Month { get; init; }
    public decimal Amount { get; init; }

    public static explicit operator CreateBudgetCellCommand(CreateBudgetCellRequest request)
        => new(request.BudgetRowId, request.Month, request.Amount);
}
