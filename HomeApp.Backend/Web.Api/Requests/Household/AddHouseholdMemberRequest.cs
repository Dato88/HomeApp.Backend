using Application.Features.Households.Commands;

namespace Web.Api.Requests.Household;

public sealed record AddHouseholdMemberRequest
{
    public int HouseholdId { get; init; }
    public string? Email { get; init; }
    public int? PersonId { get; init; }

    public static explicit operator AddHouseholdMemberCommand(AddHouseholdMemberRequest request)
        => new(request.HouseholdId, request.Email, request.PersonId);
}
