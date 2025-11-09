using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetRow : AuditableEntity
{
    public int BudgetRowId { get; set; }

    public int BudgetGroupId { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = default!;

    public virtual BudgetGroup? BudgetGroup { get; set; }
    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
}
