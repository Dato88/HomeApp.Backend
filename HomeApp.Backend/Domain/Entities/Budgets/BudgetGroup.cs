using Domain.Entities.Budgets.Enums;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetGroup : IAudited
{
    public int BudgetGroupId { get; set; }

    public int BudgetId { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = default!;
    public BudgetGroupType BudgetGroupType { get; set; }

    public virtual Budget? Budget { get; set; }
    public virtual ICollection<BudgetRow> BudgetRows { get; set; } = new HashSet<BudgetRow>();

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
