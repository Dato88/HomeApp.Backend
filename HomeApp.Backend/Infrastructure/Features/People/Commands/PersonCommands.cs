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
    IAppLogger<PersonCommands> logger) : IPersonCommands
{
    public async Task<Result> DeletePersonAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return Result.Failure(PersonErrors.DeleteFailed(id));

        var person = await dbContext.People.FindAsync(id, cancellationToken);
        if (person == null)
            return Result.Failure(PersonErrors.NotFoundById(id));

        dbContext.People.Remove(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Deleted person: {id}");

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
            .SelectMany(r => r.Errors)
            .ToList();

        var usernameCheck =
            await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);

        if (usernameCheck.IsFailure)
            validationErrors.AddRange(usernameCheck.Errors);

        if (validationErrors.Any())
        {
            foreach (var error in validationErrors)
                logger.LogWarning($"Validation failed: {error.Description}");

            return Result.Failure<int>(validationErrors.ToArray());
        }

        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Created person: {person.Id}");

        return Result.Success(person.Id);
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
            .SelectMany(r => r.Errors));

        var existingUser = await dbContext.People.FindAsync(person.Id, cancellationToken);
        if (existingUser == null)
            errors.Add(PersonErrors.NotFoundById(person.Id));

        if (existingUser != null && person.Username != existingUser.Username)
        {
            var usernameCheck =
                await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
            if (usernameCheck.IsFailure)
                errors.AddRange(usernameCheck.Errors);
        }

        if (errors.Any())
        {
            foreach (var error in errors)
                logger.LogWarning($"Update validation failed: {error.Description}");

            return Result.Failure(errors.ToArray());
        }

        existingUser!.Username = person.Username;
        existingUser.FirstName = person.FirstName;
        existingUser.LastName = person.LastName;
        existingUser.Email = person.Email;

        dbContext.People.Update(existingUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Updated person: {person.Id}");

        return Result.Success();
    }
}
