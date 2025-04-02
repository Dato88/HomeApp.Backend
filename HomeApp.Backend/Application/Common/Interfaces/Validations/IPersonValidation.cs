using Domain.Entities.People;

namespace Application.Common.Interfaces.Validations;

public interface IPersonValidation
{
    /// <summary>
    ///     Checks if the provided email is valid.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>True if the email is valid, false otherwise.</returns>
    bool IsValidEmail(string email);

    /// <summary>
    ///     Validates the format of the provided email.
    ///     <para>
    ///         <code>Throws a ValidationException if the email is invalid.</code>
    ///     </para>
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    ///     Thrown when the email is invalid.
    /// </exception>
    void ValidateEmailFormat(string email);

    /// <summary>
    ///     Validates the maximum length of the user's properties.
    ///     <para>
    ///         <code>Throws a ValidationException if any of the user's properties exceed the maximum length.</code>
    ///     </para>
    /// </summary>
    /// <param name="person">The user to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    ///     Thrown when any of the user's properties exceed the maximum length.
    /// </exception>
    void ValidateMaxLength(Person person);

    /// <summary>
    ///     Validates the required properties of the provided user.
    ///     <para>
    ///         <code>Throws a ValidationException if any of the required properties are missing.</code>
    ///     </para>
    /// </summary>
    /// <param name="person">The user to validate.</param>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    ///     Thrown when any of the required properties are missing.
    /// </exception>
    void ValidateRequiredProperties(Person person);

    /// <summary>
    ///     Validates that the provided username does not already exist.
    ///     <para>
    ///         <code>Throws an InvalidOperationException if the username already exists.</code>
    ///     </para>
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown when the username already exists.
    /// </exception>
    Task ValidatePersonnameDoesNotExistAsync(string username, CancellationToken cancellationToken);
}
