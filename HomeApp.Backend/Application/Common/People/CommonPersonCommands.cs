using Application.Common.Interfaces.People;
using Application.Common.People.Validations.Interfaces;
using Domain.Entities.People;
using Domain.PredefinedMessages;
using Infrastructure.Database;
using Infrastructure.Logger;
using Microsoft.Extensions.Logging;

namespace Application.Common.People;

public class CommonPersonCommands(
    HomeAppContext dbContext,
    IPersonValidation personValidation,
    ILogger<CommonPersonCommands> logger) : LoggerExtension<CommonPersonCommands>(logger), ICommonPersonCommands
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

            LogInformation($"Deleting person: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting person failed: {ex.Message}", DateTime.Now);
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

            LogInformation($"Creating person: {person}", DateTime.Now);

            return person.Id;
        }
        catch (Exception ex)
        {
            LogError($"Creating person failed: {ex.Message}", DateTime.Now);

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

            LogInformation($"Updating person: {person}", DateTime.Now);

            return true;
        }
        catch (Exception ex)
        {
            LogError($"Updating person failed: {ex.Message}", DateTime.Now);

            return false;
        }
    }
}
