using HomeApp.Library.Crud;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace HomeApp.Library.Validation
{
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

        public async Task ValidateUsernameDoesNotExistAsync(string username)
        {
            if (await _context.Users.AnyAsync(a => a.Username == username))
                throw new InvalidOperationException(UserMessage.UserAlreadyExists);
        }

        public void ValidateEmailFormat(string email)
        {
            if (!IsValidEmail(email))
                throw new ValidationException(UserMessage.InvalidEmail);
        }

        public void ValidateRequiredProperties(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Email))
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

        public void ValidateMaxLength(User user)
        {
            if (user.Username.Length > 150 ||
                user.FirstName.Length > 150 ||
                user.LastName.Length > 150 ||
                user.Password.Length > 150 ||
                user.Email.Length > 150)
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
}
