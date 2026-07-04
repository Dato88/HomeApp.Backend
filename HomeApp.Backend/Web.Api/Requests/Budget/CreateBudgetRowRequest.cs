using Application.Features.Budgets.Commands.Create;

namespace Web.Api.Requests.Budget;

public record CreateBudgetRowRequest
{
    public int BudgetGroupId { get; init; }
    public int Index { get; init; }
    public string Title { get; init; } = default!;
    public int? CategoryId { get; init; }

    public static explicit operator CreateBudgetRowCommand(CreateBudgetRowRequest request)
        => new(request.BudgetGroupId, request.Index, request.Title, request.CategoryId);
}
