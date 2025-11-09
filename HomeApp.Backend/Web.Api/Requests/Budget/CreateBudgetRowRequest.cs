using Application.Features.Budgets.Commands.Create;

namespace Web.Api.Requests.Budget;

public record CreateBudgetRowRequest
{
    public int BudgetGroupId { get; init; }
    public int Index { get; init; }
    public string Title { get; init; } = default!;

    public static explicit operator CreateBudgetRowCommand(CreateBudgetRowRequest request)
        => new(request.BudgetGroupId, request.Index, request.Title);
}
