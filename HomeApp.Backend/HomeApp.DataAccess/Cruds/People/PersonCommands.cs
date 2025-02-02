using HomeApp.DataAccess.Cruds.Interfaces.People;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Validations.Interfaces;

namespace HomeApp.DataAccess.Cruds.People;

public class PersonCommands(HomeAppContext dbContext, IPersonValidation personValidation)
    : BaseCommands<Person>(dbContext), IPersonCommands
{
    private readonly IPersonValidation _personValidation = personValidation;

    public override async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var user = await DbContext.People.FindAsync(id, cancellationToken);

        if (user == null)
            throw new InvalidOperationException(PersonMessage.PersonNotFound);

        DbContext.People.Remove(user);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public override async Task<bool> UpdateAsync(Person person, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(person);

        _personValidation.ValidateRequiredProperties(person);
        _personValidation.ValidateMaxLength(person);
        _personValidation.ValidateEmailFormat(person.Email);

        var existingUser = await DbContext.People.FindAsync(person.Id, cancellationToken);

        if (existingUser == null)
            throw new InvalidOperationException(PersonMessage.PersonNotFound);

        if (person.Username != existingUser.Username)
            await _personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);

        existingUser.Username = person.Username;
        existingUser.FirstName = person.FirstName;
        existingUser.LastName = person.LastName;
        existingUser.Email = person.Email;

        DbContext.People.Update(existingUser);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<int> CreateAsync(Person person, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(person);

        _personValidation.ValidateRequiredProperties(person);
        _personValidation.ValidateMaxLength(person);
        await _personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
        _personValidation.ValidateEmailFormat(person.Email);

        DbContext.People.Add(person);

        await DbContext.SaveChangesAsync(cancellationToken);

        return person.Id;
    }
}
