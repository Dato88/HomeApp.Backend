namespace HomeApp.Library.Validation.Interfaces
{
    public interface IUserValidation
    {
        bool IsValidEmail(string email);
        void ValidateEmailFormat(string email);
        void ValidateLastLoginDate(DateTime? lastLogin);
        void ValidateMaxLength(User user);
        void ValidatePasswordStrength(string password);
        void ValidateRequiredProperties(User user);
        Task ValidateUsernameDoesNotExistAsync(string username);
    }
}