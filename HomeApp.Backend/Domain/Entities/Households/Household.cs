using SharedKernel;

namespace Domain.Entities.Households;

public class Household : AuditableEntity
{
    public int HouseholdId { get; set; }

    public string Name { get; set; } = default!;

    public virtual ICollection<HouseholdMember> Members { get; set; } = new HashSet<HouseholdMember>();
}
