using Domain.Entities.People;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Domain.Entities.Budgets;

public class BudgetCell : BaseClass
{
    public int BudgetRowId { get; set; }
    public int BudgetColumnId { get; set; }
    public int BudgetGroupId { get; set; }
    public PersonId PersonId { get; set; }
    public int Year { get; set; }
    public string Name { get; set; }

    public virtual BudgetColumn BudgetColumn { get; set; }
    public virtual BudgetGroup BudgetGroup { get; set; }
    public virtual BudgetRow BudgetRow { get; set; }
    public virtual Person Person { get; set; }
}
