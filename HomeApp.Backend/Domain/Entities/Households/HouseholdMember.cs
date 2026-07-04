using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Households;

public class HouseholdMember : AuditableEntity
{
    public int HouseholdMemberId { get; set; }

    public int HouseholdId { get; set; }
    public int PersonId { get; set; }

    public virtual Household? Household { get; set; }
    public virtual Person? Person { get; set; }
}
