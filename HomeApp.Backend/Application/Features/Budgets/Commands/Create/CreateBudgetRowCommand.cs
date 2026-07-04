using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Create;

public sealed record CreateBudgetRowCommand(
    int BudgetGroupId,
    int Index,
    string Name,
    int? CategoryId = null) : IRequest<Result<int>>
{
    public static explicit operator BudgetRow(CreateBudgetRowCommand item) =>
        new()
        {
            BudgetGroupId = item.BudgetGroupId, Index = item.Index, Title = item.Name, CategoryId = item.CategoryId
        };
}
