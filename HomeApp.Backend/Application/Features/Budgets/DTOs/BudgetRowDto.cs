using Domain.Entities.Budgets;

namespace Application.Features.Budgets.DTOs;

public class BudgetRowDto
{
    public int BudgetRowId { get; set; }

    public int BudgetGroupId { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = default!;

    public static explicit operator BudgetRowDto(BudgetRow entity) =>
        new()
        {
            BudgetRowId = entity.BudgetRowId,
            BudgetGroupId = entity.BudgetGroupId,
            Index = entity.Index,
            Name = entity.Name,
        };
}
