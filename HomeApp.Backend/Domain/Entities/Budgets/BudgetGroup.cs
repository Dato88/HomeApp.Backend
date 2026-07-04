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

    // Target share of total income in percent (e.g. 30 for the 30/10/30/30 rule)
    public decimal? TargetPercent { get; set; }

    public virtual Budget? Budget { get; set; }
    public virtual ICollection<BudgetRow> BudgetRows { get; set; } = new HashSet<BudgetRow>();
}
