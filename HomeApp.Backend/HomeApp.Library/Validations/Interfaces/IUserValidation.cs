namespace HomeApp.Library.Validations.Interfaces;

public interface IUserValidation
{
    /// <summary>
    /// Checks if the provided email is valid.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>True if the email is valid, false otherwise.</returns>
    bool IsValidEmail(string email);

    /// <summary>
    /// Validates the format of the provided email.
    /// <para>
    /// <code>Throws a ValidationException if the email is invalid.</code>
    /// </para>
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// Thrown when the email is invalid.
    /// </exception>
    void ValidateEmailFormat(string email);

    /// <summary>
    /// Validates the last login date.
    /// <para>
    /// <code>Throws a ValidationException if the last login date is in the future.</code>
    /// </para>
    /// </summary>
    /// <param name="lastLogin">The last login date to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// Thrown when the last login date is in the future.
    /// </exception>
    void ValidateLastLoginDate(DateTime? lastLogin);

    /// <summary>
    /// Validates the maximum length of the user's properties.
    /// <para>
    /// <code>Throws a ValidationException if any of the user's properties exceed the maximum length.</code>
    /// </para>
    /// </summary>
    /// <param name="person">The user to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// Thrown when any of the user's properties exceed the maximum length.
    /// </exception>
    void ValidateMaxLength(Person person);

    /// <summary>
    /// Validates the strength of the provided password.
    /// <para>
    /// <code>Throws a ValidationException if the password does not meet the strength requirements.</code>
    /// </para>
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// Thrown when the password does not meet the strength requirements.
    /// </exception>
    void ValidatePasswordStrength(string password);

    /// <summary>
    /// Validates the required properties of the provided user.
    /// <para>
    /// <code>Throws a ValidationException if any of the required properties are missing.</code>
    /// </para>
    /// </summary>
    /// <param name="person">The user to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// Thrown when any of the required properties are missing.
    /// </exception>
    void ValidateRequiredProperties(Person person);

    /// <summary>
    /// Validates that the provided username does not already exist.
    /// <para>
    /// <code>Throws an InvalidOperationException if the username already exists.</code>
    /// </para>
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when the username already exists.
    /// </exception>
    Task ValidateUsernameDoesNotExistAsync(string username, CancellationToken cancellationToken);
}
