using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using HomeApp.Library.Cruds;

namespace HomeApp.Library.Validations;

public class UserValidation(HomeAppContext context) : BaseContext(context), IUserValidation
{
    public bool IsValidEmail(string email)
    {
        try
        {
            MailAddress addr = new(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public async Task ValidateUsernameDoesNotExistAsync(string username, CancellationToken cancellationToken)
    {
        if (await _context.People.AnyAsync(a => a.Username == username, cancellationToken))
            throw new InvalidOperationException(UserMessage.UserAlreadyExists);
    }

    public void ValidateEmailFormat(string email)
    {
        if (!IsValidEmail(email))
            throw new ValidationException(UserMessage.InvalidEmail);
    }

    public void ValidateRequiredProperties(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.Username) ||
            string.IsNullOrWhiteSpace(person.FirstName) ||
            string.IsNullOrWhiteSpace(person.LastName) ||
            string.IsNullOrWhiteSpace(person.Email) ||
            string.IsNullOrWhiteSpace(person.UserId))
            throw new ValidationException(UserMessage.PropertiesMissing);
    }

    public void ValidateMaxLength(Person person)
    {
        if (person.Username.Length > 150 ||
            person.FirstName.Length > 150 ||
            person.LastName.Length > 150 ||
            person.Email.Length > 150 ||
            person.UserId.Length < 36)
            throw new ValidationException(UserMessage.MaxLengthExeed);
    }
}
