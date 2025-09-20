using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetCell : IAudited
{
    public int BudgetCellId { get; set; }

    public int BudgetRowId { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }

    public virtual BudgetRow BudgetRow { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
