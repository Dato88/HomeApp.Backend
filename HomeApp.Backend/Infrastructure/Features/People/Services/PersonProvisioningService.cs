using System.Security.Claims;
using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Domain.Entities.People;
using Infrastructure.Database;
using Infrastructure.Services.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Features.People.Services;

internal sealed class PersonProvisioningService(
    HomeAppContext dbContext,
    IPersonIdCache personIdCache,
    IAppLogger<PersonProvisioningService> logger) : IPersonProvisioningService
{
    public async Task<int> EnsurePersonAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var userId = user.GetKeycloakUserId()?.ToString()
                     ?? throw new InvalidOperationException("Authenticated user is missing sub claim.");

        if (personIdCache.TryGetPersonId(userId, out var cachedPersonId))
            return cachedPersonId;

        var existingPerson = await dbContext.People
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (existingPerson is not null)
        {
            personIdCache.SetPersonId(userId, existingPerson.PersonId);
            return existingPerson.PersonId;
        }

        var personId = await CreatePersonAsync(user, userId, cancellationToken);
        personIdCache.SetPersonId(userId, personId);
        return personId;
    }

    private async Task<int> CreatePersonAsync(
        ClaimsPrincipal user,
        string userId,
        CancellationToken cancellationToken)
    {
        var email = user.GetUserEmail()?.Value ?? $"{userId}@homeapp.local";
        var firstName = user.GetFirstName() ?? "User";
        var lastName = user.GetLastName() ?? "Unknown";
        var username = user.GetPreferredUsername() ?? email.Split('@')[0];

        if (user.GetUserEmail() is null)
            logger.LogWarning($"Access token for Keycloak user {userId} is missing email claim.");

        var person = new Person
        {
            UserId = userId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Username = username
        };

        try
        {
            return await SaveNewPersonAsync(person, userId, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            var existingPerson = await dbContext.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

            if (existingPerson is not null)
                return existingPerson.PersonId;

            if (person.Username is not null)
            {
                person.Username = null;

                try
                {
                    return await SaveNewPersonAsync(person, userId, cancellationToken);
                }
                catch (DbUpdateException retryEx) when (IsUniqueConstraintViolation(retryEx))
                {
                    var racedPerson = await dbContext.People
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

                    if (racedPerson is not null)
                        return racedPerson.PersonId;

                    throw;
                }
            }

            throw;
        }
    }

    private async Task<int> SaveNewPersonAsync(Person person, string userId, CancellationToken cancellationToken)
    {
        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        person.CreatedById = person.PersonId;
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Provisioned person {person.PersonId} for Keycloak user {userId}");

        return person.PersonId;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.OrdinalIgnoreCase) == true;
}
