using Application.Features.Budgets.Commands.Create;
using Domain.Entities.Budgets.Enums;

namespace Web.Api.Requests.Budget;

public sealed record CreateBudgetGroupRequest
{
    public int BudgetId { get; init; }
    public int Index { get; init; }
    public string Title { get; init; } = string.Empty;
    public BudgetGroupType BudgetGroupType { get; init; }

    public static explicit operator CreateBudgetGroupCommand(CreateBudgetGroupRequest request)
        => new(request.BudgetId, request.Index, request.Title, request.BudgetGroupType);
}
