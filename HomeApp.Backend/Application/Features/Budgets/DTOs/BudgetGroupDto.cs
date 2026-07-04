using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;

namespace Application.Features.Budgets.DTOs;

public class BudgetGroupDto
{
    public int BudgetGroupId { get; set; }

    public int BudgetId { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = default!;
    public BudgetGroupType BudgetGroupType { get; set; }

    public static explicit operator BudgetGroupDto(BudgetGroup entity) =>
        new()
        {
            BudgetGroupId = entity.BudgetGroupId,
            BudgetId = entity.BudgetId,
            Index = entity.Index,
            Title = entity.Title,
            BudgetGroupType = entity.BudgetGroupType
        };
}
