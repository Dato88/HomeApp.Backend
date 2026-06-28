using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Features.People.Commands;
using Application.Features.People.Validations;
using Domain.Entities.People;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Features.People.Commands;

public sealed class PersonCommands(
    HomeAppContext dbContext,
    IPersonValidation personValidation,
    IPersonIdCache personIdCache,
    IAppLogger<PersonCommands> logger) : IPersonCommands
{
    public async Task<Result> DeletePersonAsync(int personId, CancellationToken cancellationToken)
    {
        if (personId <= 0)
            return Result.Failure(PersonErrors.DeleteFailed(personId));

        var person = await dbContext.People.FindAsync(personId, cancellationToken);
        if (person == null)
            return Result.Failure(PersonErrors.NotFoundById(personId));

        personIdCache.Remove(person.UserId);

        dbContext.People.Remove(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Deleted person: {personId}");

        return Result.Success();
    }

    public async Task<Result<int>> CreatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        if (person is null)
            return Result.Failure<int>(PersonErrors.CreateFailedWithMessage("Person is null"));

        var validationResults = new[]
        {
            personValidation.ValidateRequiredProperties(person), personValidation.ValidateMaxLength(person),
            personValidation.ValidateEmailFormat(person.Email)
        };

        var validationErrors = validationResults
            .Where(r => r.IsFailure)
            .Select(r => r.Error)
            .ToList();

        var usernameCheck =
            await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);

        if (usernameCheck.IsFailure)
            validationErrors.Add(usernameCheck.Error);

        if (validationErrors.Any())
        {
            foreach (var error in validationErrors)
                logger.LogWarning($"Validation failed: {error.Description}");

            return Result.Failure<int>(validationErrors.First());
        }

        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Created person: {person.PersonId}");

        return Result.Success(person.PersonId);
    }

    public async Task<Result> UpdatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        if (person is null)
            return Result.Failure(PersonErrors.UpdateFailedWithMessage("Person is null"));

        var errors = new List<Error>();

        var validationResults = new[]
        {
            personValidation.ValidateRequiredProperties(person), personValidation.ValidateMaxLength(person),
            personValidation.ValidateEmailFormat(person.Email)
        };

        errors.AddRange(validationResults
            .Where(r => r.IsFailure)
            .Select(r => r.Error));

        var existingUser = await dbContext.People.FindAsync(person.PersonId, cancellationToken);
        if (existingUser == null)
            errors.Add(PersonErrors.NotFoundById(person.PersonId));

        if (existingUser != null && person.Username != existingUser.Username)
        {
            var usernameCheck =
                await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
            if (usernameCheck.IsFailure)
                errors.Add(usernameCheck.Error);
        }

        if (errors.Any())
        {
            foreach (var error in errors)
                logger.LogWarning($"Update validation failed: {error.Description}");

            return Result.Failure(errors.First());
        }

        existingUser!.Username = person.Username;
        existingUser.FirstName = person.FirstName;
        existingUser.LastName = person.LastName;
        existingUser.Email = person.Email;

        dbContext.People.Update(existingUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Updated person: {person.PersonId}");

        return Result.Success();
    }
}
