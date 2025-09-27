using Application.Features.Budgets.Commands.Create;

namespace Web.Api.Requests.Budget;

public record CreateBudgetRowRequest
{
    public int BudgetGroupId { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = default!;

    public static explicit operator CreateBudgetRowCommand(CreateBudgetRowRequest request)
        => new(request.BudgetGroupId, request.Index, request.Name);
}
