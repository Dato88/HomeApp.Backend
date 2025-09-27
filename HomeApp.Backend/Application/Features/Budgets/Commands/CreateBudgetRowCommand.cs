using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed record CreateBudgetRowCommand(
    int BudgetGroupId,
    int Index,
    string Name) : IRequest<Result<int>>
{
    public static explicit operator BudgetRow(CreateBudgetRowCommand item) =>
        new() { BudgetGroupId = item.BudgetGroupId, Index = item.Index, Name = item.Name };
}
