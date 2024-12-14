using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using HomeApp.Library.Cruds;
using HomeApp.Library.Validations.Interfaces;

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
        if (await _context.Users.AnyAsync(a => a.Username == username, cancellationToken))
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
            string.IsNullOrWhiteSpace(person.Password) ||
            string.IsNullOrWhiteSpace(person.Email))
        {
            throw new ValidationException(UserMessage.PropertiesMissing);
        }
    }

    public void ValidatePasswordStrength(string password)
    {
        if (password.Length < 8)
            throw new ValidationException(UserMessage.PasswordShort);

        // Überprüfen auf Großbuchstaben
        if (!password.Any(char.IsUpper))
            throw new ValidationException(UserMessage.PasswordUppercaseMissing);

        // Überprüfen auf Kleinbuchstaben
        if (!password.Any(char.IsLower))
            throw new ValidationException(UserMessage.PasswordLowercaseMissing);

        // Überprüfen auf Zahlen
        if (!password.Any(char.IsDigit))
            throw new ValidationException(UserMessage.PasswordDigitMissing);

        // Überprüfen auf Sonderzeichen
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            throw new ValidationException(UserMessage.PasswordSpecialCharMissing);
    }

    public void ValidateMaxLength(Person person)
    {
        if (person.Username.Length > 150 ||
            person.FirstName.Length > 150 ||
            person.LastName.Length > 150 ||
            person.Password.Length > 150 ||
            person.Email.Length > 150)
        {
            throw new ValidationException(UserMessage.MaxLengthExeed);
        }
    }

    public void ValidateLastLoginDate(DateTime? lastLogin)
    {
        if (lastLogin.HasValue && lastLogin.Value > DateTime.Now)
            throw new ValidationException(UserMessage.InvalidLoginDate);
    }
}
