using Application.Features.Finance.Accounts.Commands;
using Domain.Entities.Finance.Enums;

namespace Web.Api.Requests.Finance;

public sealed record CreateAccountRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Iban { get; init; }
    public string? Bic { get; init; }
    public AccountType AccountType { get; init; }
    public string? CurrencyCode { get; init; }
    public string? Description { get; init; }
    public List<int>? HouseholdIds { get; init; }

    public static explicit operator CreateAccountCommand(CreateAccountRequest request)
        => new(request.Name, request.Iban, request.Bic, request.AccountType, request.CurrencyCode,
            request.Description, request.HouseholdIds);
}
