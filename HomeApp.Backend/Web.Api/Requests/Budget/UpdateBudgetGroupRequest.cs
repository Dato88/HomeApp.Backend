using Application.Features.Budgets.Commands.Update;
using Domain.Entities.Budgets.Enums;

namespace Web.Api.Requests.Budget;

public sealed record UpdateBudgetGroupRequest
{
    public int BudgetGroupId { get; init; }
    public int Index { get; init; }
    public string Title { get; init; } = string.Empty;
    public BudgetGroupType BudgetGroupType { get; init; }
    public decimal? TargetPercent { get; init; }

    public static explicit operator UpdateBudgetGroupCommand(UpdateBudgetGroupRequest request)
        => new(request.BudgetGroupId, request.Index, request.Title, request.BudgetGroupType,
            request.TargetPercent);
}
