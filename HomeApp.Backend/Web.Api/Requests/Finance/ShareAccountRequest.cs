using Application.Features.Finance.Accounts.Commands;

namespace Web.Api.Requests.Finance;

public sealed record ShareAccountRequest
{
    public int AccountId { get; init; }
    public int HouseholdId { get; init; }

    public static explicit operator ShareAccountCommand(ShareAccountRequest request)
        => new(request.AccountId, request.HouseholdId);
}
