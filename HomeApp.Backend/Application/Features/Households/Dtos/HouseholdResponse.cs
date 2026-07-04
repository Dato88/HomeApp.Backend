using Domain.Entities.Households;

namespace Application.Features.Households.Dtos;

public sealed class HouseholdResponse
{
    public int HouseholdId { get; set; }
    public string Name { get; set; } = default!;

    public IEnumerable<HouseholdMemberDto> Members { get; set; } = new List<HouseholdMemberDto>();

    public static explicit operator HouseholdResponse(Household entity) =>
        new()
        {
            HouseholdId = entity.HouseholdId,
            Name = entity.Name,
            Members = entity.Members.Select(m => (HouseholdMemberDto)m).ToList()
        };
}
