using Application.Features.Households.Commands;

namespace Web.Api.Requests.Household;

public sealed record CreateHouseholdRequest
{
    public string Name { get; init; } = string.Empty;

    public static explicit operator CreateHouseholdCommand(CreateHouseholdRequest request)
        => new(request.Name);
}
