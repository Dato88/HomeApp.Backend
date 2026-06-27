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
    IAppLogger<PersonProvisioningService> logger) : IPersonProvisioningService
{
    public async Task<int> EnsurePersonAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var personIdClaim = user.GetPersonId();
        if (personIdClaim.HasValue)
        {
            var personById = await dbContext.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PersonId == personIdClaim.Value, cancellationToken);

            if (personById is not null)
                return personById.PersonId;
        }

        var userId = user.GetUserId()?.ToString()
                     ?? throw new InvalidOperationException("Authenticated user is missing sub claim.");

        var existingPerson = await dbContext.People
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (existingPerson is not null)
            return existingPerson.PersonId;

        var email = user.GetUserEmail()?.Value ?? $"{userId}@homeapp.local";
        var firstName = user.FindFirstValue("given_name") ?? "User";
        var lastName = user.FindFirstValue("family_name") ?? "Unknown";
        var username = user.FindFirstValue("preferred_username")
                       ?? email.Split('@')[0];

        var person = new Person
        {
            UserId = userId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Username = username
        };

        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Provisioned person {person.PersonId} for Keycloak user {userId}");

        return person.PersonId;
    }
}
