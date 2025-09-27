using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed record CreateBudgetGroupCommand(
    int BudgetId,
    int Index,
    string Name,
    BudgetGroupType BudgetGroupType) : IRequest<Result<int>>
{
    public static explicit operator BudgetGroup(CreateBudgetGroupCommand item) =>
        new()
        {
            BudgetId = item.BudgetId, Index = item.Index, Name = item.Name, BudgetGroupType = item.BudgetGroupType
        };
}
