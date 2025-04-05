using Application.Abstractions.Logging;
using Application.Features.People.Commands;
using Application.Features.People.Validations;
using Domain.Entities.People;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Features.People.Commands;

public class PersonCommands(
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

        try
        {
            personValidation.ValidateRequiredProperties(person);
            personValidation.ValidateMaxLength(person);
            await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
            personValidation.ValidateEmailFormat(person.Email);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Validation failed while creating person: {ex.Message}");
            return Result.Failure<int>(PersonErrors.CreateFailedWithMessage(ex.Message));
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

        try
        {
            personValidation.ValidateRequiredProperties(person);
            personValidation.ValidateMaxLength(person);
            personValidation.ValidateEmailFormat(person.Email);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Validation failed while updating person: {ex.Message}");
            return Result.Failure(PersonErrors.UpdateFailedWithMessage(ex.Message));
        }

        var existingUser = await dbContext.People.FindAsync(person.Id, cancellationToken);

        if (existingUser == null)
            return Result.Failure(PersonErrors.NotFoundById(person.Id));

        if (person.Username != existingUser.Username)
            try
            {
                await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure(PersonErrors.UpdateFailedWithMessage(ex.Message));
            }

        existingUser.Username = person.Username;
        existingUser.FirstName = person.FirstName;
        existingUser.LastName = person.LastName;
        existingUser.Email = person.Email;

        dbContext.People.Update(existingUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Updated person: {person.Id}");

        return Result.Success();
    }
}
