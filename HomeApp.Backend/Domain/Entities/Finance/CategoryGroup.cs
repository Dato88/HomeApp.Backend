using Domain.Entities.Finance.Enums;
using Domain.Entities.Households;
using SharedKernel;

namespace Domain.Entities.Finance;

public class CategoryGroup : AuditableEntity
{
    public int CategoryGroupId { get; set; }

    public int HouseholdId { get; set; }
    public string Name { get; set; } = default!;
    public CategoryType CategoryGroupType { get; set; }

    // Target share of total income in percent (e.g. 30 for the 30/10/30/30 rule)
    public decimal? TargetPercent { get; set; }

    public virtual Household? Household { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
}
