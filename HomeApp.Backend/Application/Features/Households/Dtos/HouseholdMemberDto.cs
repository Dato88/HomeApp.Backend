using Domain.Entities.Households;

namespace Application.Features.Households.Dtos;

public sealed class HouseholdMemberDto
{
    public int PersonId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;

    public static explicit operator HouseholdMemberDto(HouseholdMember entity) =>
        new()
        {
            PersonId = entity.PersonId,
            FirstName = entity.Person?.FirstName ?? string.Empty,
            LastName = entity.Person?.LastName ?? string.Empty,
            Email = entity.Person?.Email ?? string.Empty
        };
}
