using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetCell : AuditableEntity
{
    public int BudgetCellId { get; set; }

    public int BudgetRowId { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }

    public virtual BudgetRow? BudgetRow { get; set; }
}
