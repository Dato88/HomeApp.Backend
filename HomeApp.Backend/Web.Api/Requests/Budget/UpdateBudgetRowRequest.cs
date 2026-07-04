using Application.Features.Budgets.Commands.Update;

namespace Web.Api.Requests.Budget;

public sealed record UpdateBudgetRowRequest
{
    public int BudgetRowId { get; init; }
    public int Index { get; init; }
    public string Title { get; init; } = string.Empty;
    public int? CategoryId { get; init; }

    public static explicit operator UpdateBudgetRowCommand(UpdateBudgetRowRequest request)
        => new(request.BudgetRowId, request.Index, request.Title, request.CategoryId);
}
