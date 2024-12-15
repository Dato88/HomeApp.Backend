using HomeApp.Library.Models.Data_Transfer_Objects.PersonDtos;

namespace HomeApp.Library.Cruds;

public class PersonCrud(HomeAppContext context, IUserValidation userValidation) : BaseCrud<Person>(context, null)
{
    private readonly IUserValidation _userValidation = userValidation;

    public override async Task CreateAsync(Person person, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(person);

        _userValidation.ValidateRequiredProperties(person);
        _userValidation.ValidateMaxLength(person);
        await _userValidation.ValidateUsernameDoesNotExistAsync(person.Username, cancellationToken);
        _userValidation.ValidateEmailFormat(person.Email);

        _context.People.Add(person);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.People.FindAsync(userId, cancellationToken);

        if (user == null)
            throw new InvalidOperationException(UserMessage.UserNotFound);

        _context.People.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public override async Task<PersonDto> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _context.People.FindAsync(id, cancellationToken);

        if (user is null)
            throw new InvalidOperationException(UserMessage.UserNotFound);

        return user;
    }

    public override async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken) =>
        (await _context.People.ToListAsync(cancellationToken)).Select(s => (PersonDto)s);

    public override async Task UpdateAsync(Person person, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(person);

        _userValidation.ValidateRequiredProperties(person);
        _userValidation.ValidateMaxLength(person);
        _userValidation.ValidateEmailFormat(person.Email);

        var existingUser = await _context.People.FindAsync(person.Id, cancellationToken);

        if (existingUser == null)
            throw new InvalidOperationException(UserMessage.UserNotFound);

        if (person.Username != existingUser.Username)
            await _userValidation.ValidateUsernameDoesNotExistAsync(person.Username, cancellationToken);

        existingUser.Username = person.Username;
        existingUser.FirstName = person.FirstName;
        existingUser.LastName = person.LastName;
        existingUser.Email = person.Email;

        _context.People.Update(existingUser);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
