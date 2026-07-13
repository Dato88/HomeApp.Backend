using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed record UpdateAccountCommand(
    int AccountId,
    string Name,
    string? Iban,
    string? Bic,
    AccountType AccountType,
    string? CurrencyCode,
    string? Description,
    bool IsActive,
    DateOnly? DeactivatedFrom = null) : IRequest<Result<int>>
{
    public static explicit operator Account(UpdateAccountCommand item) =>
        new()
        {
            AccountId = item.AccountId,
            Name = item.Name,
            Iban = string.IsNullOrWhiteSpace(item.Iban) ? null : Domain.ValueObjects.Iban.Normalize(item.Iban),
            Bic = string.IsNullOrWhiteSpace(item.Bic) ? null : item.Bic.Trim().ToUpperInvariant(),
            AccountType = item.AccountType,
            CurrencyCode = string.IsNullOrWhiteSpace(item.CurrencyCode)
                ? "EUR"
                : item.CurrencyCode.Trim().ToUpperInvariant(),
            Description = item.Description,
            IsActive = item.IsActive,
            DeactivatedFrom = item.DeactivatedFrom
        };
}
