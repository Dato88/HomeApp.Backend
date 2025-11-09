using Domain.Entities.Budgets.Enums;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetGroup : AuditableEntity
{
    public int BudgetGroupId { get; set; }

    public int BudgetId { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = default!;
    public BudgetGroupType BudgetGroupType { get; set; }

    public virtual Budget? Budget { get; set; }
    public virtual ICollection<BudgetRow> BudgetRows { get; set; } = new HashSet<BudgetRow>();
}
