using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;

namespace Application.Features.Finance.Dtos;

public sealed class AccountDto
{
    public int AccountId { get; set; }
    public int PersonId { get; set; }
    public string Name { get; set; } = default!;
    public string? Iban { get; set; }
    public string? Bic { get; set; }
    public AccountType AccountType { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsOwner { get; set; }
    public IEnumerable<int> SharedHouseholdIds { get; set; } = new List<int>();

    public static explicit operator AccountDto(Account entity) =>
        new()
        {
            AccountId = entity.AccountId,
            PersonId = entity.PersonId,
            Name = entity.Name,
            Iban = entity.Iban,
            Bic = entity.Bic,
            AccountType = entity.AccountType,
            CurrencyCode = entity.CurrencyCode,
            Description = entity.Description,
            IsActive = entity.IsActive,
            SharedHouseholdIds = entity.AccountHouseholds.Select(ah => ah.HouseholdId).ToList()
        };
}
