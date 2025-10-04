using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetRow : IAudited
{
    public int BudgetRowId { get; set; }

    public int BudgetGroupId { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = default!;

    public virtual BudgetGroup? BudgetGroup { get; set; }
    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
