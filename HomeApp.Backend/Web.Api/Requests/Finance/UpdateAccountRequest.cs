using Application.Features.Finance.Accounts.Commands;
using Domain.Entities.Finance.Enums;

namespace Web.Api.Requests.Finance;

public sealed record UpdateAccountRequest
{
    public int AccountId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Iban { get; init; }
    public string? Bic { get; init; }
    public AccountType AccountType { get; init; }
    public string? CurrencyCode { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;

    public static explicit operator UpdateAccountCommand(UpdateAccountRequest request)
        => new(request.AccountId, request.Name, request.Iban, request.Bic, request.AccountType,
            request.CurrencyCode, request.Description, request.IsActive);
}
