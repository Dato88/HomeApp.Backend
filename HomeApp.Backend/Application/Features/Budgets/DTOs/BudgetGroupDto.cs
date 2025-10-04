using Domain.Entities.Budgets;

namespace Application.Features.Budgets.DTOs;

public class BudgetGroupDto
{
    public int BudgetGroupId { get; set; }

    public int BudgetId { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = default!;

    public static explicit operator BudgetGroupDto(BudgetGroup entity) =>
        new()
        {
            BudgetGroupId = entity.BudgetGroupId,
            BudgetId = entity.BudgetId,
            Index = entity.Index,
            Name = entity.Title
        };
}
