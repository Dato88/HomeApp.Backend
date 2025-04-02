using Application.Abstractions.Data;
using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.Common.Interfaces.Validations;
using Domain.Entities.People;
using Domain.PredefinedMessages;

namespace Application.Common.People;

public class CommonPersonCommands(
    IHomeAppContext dbContext,
    IPersonValidation personValidation,
    IAppLogger<CommonPersonCommands> logger) : ICommonPersonCommands
{
    public async Task DeletePersonAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await dbContext.People.FindAsync(id, cancellationToken);

            if (user == null)
                throw new InvalidOperationException(PersonMessage.PersonNotFound);

            dbContext.People.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Deleting person: {id}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Deleting person failed: {ex.Message}");
        }
    }

    public async Task<int> CreatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(person);

            personValidation.ValidateRequiredProperties(person);
            personValidation.ValidateMaxLength(person);
            await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
            personValidation.ValidateEmailFormat(person.Email);

            dbContext.People.Add(person);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Creating person: {person}");

            return person.Id;
        }
        catch (Exception ex)
        {
            logger.LogError($"Creating person failed: {ex.Message}");

            return 0;
        }
    }

    public async Task<bool> UpdatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(person);

            personValidation.ValidateRequiredProperties(person);
            personValidation.ValidateMaxLength(person);
            personValidation.ValidateEmailFormat(person.Email);

            var existingUser = await dbContext.People.FindAsync(person.Id, cancellationToken);

            if (existingUser == null)
                throw new InvalidOperationException(PersonMessage.PersonNotFound);

            if (person.Username != existingUser.Username)
                await personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);

            existingUser.Username = person.Username;
            existingUser.FirstName = person.FirstName;
            existingUser.LastName = person.LastName;
            existingUser.Email = person.Email;

            dbContext.People.Update(existingUser);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Updating person: {person}");

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Updating person failed: {ex.Message}");

            return false;
        }
    }
}
