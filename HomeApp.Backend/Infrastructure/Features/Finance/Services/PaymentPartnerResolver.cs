using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Features.Finance.Services;

// Resolves the raw partner strings of a transaction to a deduplicated PaymentPartner of the
// account owner. Matching rule (mirrored by the backfill in the paymentPartners migration):
// 1. Row has an IBAN -> match only on (PersonId, Iban); no hit -> create a partner with that IBAN.
//    An IBAN row is never attached to a name match - the IBAN identity is stronger.
// 2. Row has no IBAN -> match on (PersonId, NormalizedName) against all partners; the oldest
//    (lowest id) wins. No hit -> create an IBAN-less partner.
// 3. Neither name nor IBAN -> no partner.
public sealed class PaymentPartnerResolver(HomeAppContext dbContext)
{
    private readonly HomeAppContext _dbContext = dbContext;

    // One query per batch: loads only the owner's partners whose keys appear in the batch,
    // plus the owner's account IBANs for self-transfer detection.
    public async Task<PaymentPartnerScope> LoadScopeAsync(int ownerPersonId,
        IReadOnlyCollection<(string? Name, string? Iban)> keys, CancellationToken cancellationToken)
    {
        var ibans = keys
            .Select(k => NormalizeIban(k.Iban))
            .OfType<string>()
            .Distinct()
            .ToList();

        var names = keys
            .Select(k => Domain.ValueObjects.PartnerName.Normalize(k.Name))
            .OfType<string>()
            .Distinct()
            .ToList();

        // Ordered by id so the oldest partner wins the name dictionary slot
        var existing = await _dbContext.PaymentPartners
            .Where(p => p.PersonId == ownerPersonId &&
                        ((p.Iban != null && ibans.Contains(p.Iban)) || names.Contains(p.NormalizedName)))
            .OrderBy(p => p.PaymentPartnerId)
            .ToListAsync(cancellationToken);

        var ownAccounts = await _dbContext.Accounts
            .AsNoTracking()
            .Where(a => a.PersonId == ownerPersonId && a.Iban != null)
            .Select(a => new { a.AccountId, a.Iban })
            .ToListAsync(cancellationToken);

        var ownAccountsByIban = new Dictionary<string, int>();

        foreach (var account in ownAccounts)
            ownAccountsByIban.TryAdd(Domain.ValueObjects.Iban.Normalize(account.Iban!), account.AccountId);

        return new PaymentPartnerScope(_dbContext, ownerPersonId, existing, ownAccountsByIban);
    }

    internal static string? NormalizeIban(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : Truncate(Domain.ValueObjects.Iban.Normalize(value), 34);

    internal static string? Truncate(string? value, int maxLength) =>
        value is null || value.Length <= maxLength ? value : value[..maxLength];
}

public sealed class PaymentPartnerScope
{
    private readonly HomeAppContext _dbContext;
    private readonly int _ownerPersonId;
    private readonly Dictionary<string, PaymentPartner> _byIban = [];
    private readonly Dictionary<string, PaymentPartner> _byName = [];
    private readonly IReadOnlyDictionary<string, int> _ownAccountsByIban;

    internal PaymentPartnerScope(HomeAppContext dbContext, int ownerPersonId,
        IReadOnlyList<PaymentPartner> existing, IReadOnlyDictionary<string, int> ownAccountsByIban)
    {
        _dbContext = dbContext;
        _ownerPersonId = ownerPersonId;
        _ownAccountsByIban = ownAccountsByIban;

        foreach (var partner in existing)
        {
            if (partner.Iban is not null)
                _byIban.TryAdd(partner.Iban, partner);

            _byName.TryAdd(partner.NormalizedName, partner);
        }
    }

    // Returns the existing or a newly created partner (added to the change tracker; the caller's
    // SaveChanges persists it). New partners are registered so later rows of the same batch reuse them.
    public PaymentPartner? Resolve(string? rawName, string? rawIban)
    {
        var iban = PaymentPartnerResolver.NormalizeIban(rawIban);
        var name = Domain.ValueObjects.PartnerName.Normalize(rawName);

        if (iban is null && name is null)
            return null;

        if (iban is not null)
        {
            if (_byIban.TryGetValue(iban, out var byIban))
            {
                // The own account may have been created after the partner - catch up on the link
                if (byIban.LinkedAccountId is null && _ownAccountsByIban.TryGetValue(iban, out var accountId))
                    byIban.LinkedAccountId = accountId;

                return byIban;
            }
        }
        else if (_byName.TryGetValue(name!, out var byName))
        {
            return byName;
        }

        var created = new PaymentPartner
        {
            PersonId = _ownerPersonId,
            DisplayName = CleanDisplayName(rawName) ?? iban!,
            NormalizedName = name ?? iban!,
            Iban = iban,
            LinkedAccountId = iban is not null && _ownAccountsByIban.TryGetValue(iban, out var linkedAccountId)
                ? linkedAccountId
                : null,
            CreatedById = _ownerPersonId
        };

        _dbContext.PaymentPartners.Add(created);

        if (iban is not null)
            _byIban[iban] = created;

        _byName.TryAdd(created.NormalizedName, created);

        return created;
    }

    // Display name = raw name with collapsed whitespace, original casing preserved
    private static string? CleanDisplayName(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : PaymentPartnerResolver.Truncate(
                string.Join(' ', value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)), 200);
}
