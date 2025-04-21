using Domain.Entities.People;
using SharedKernel;

namespace Application.Features.People.Validations;

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
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>A Result indicating success or failure.</returns>
    Result ValidateEmailFormat(string email);

    /// <summary>
    ///     Validates the maximum length of the user's properties.
    /// </summary>
    /// <param name="person">The user to validate.</param>
    /// <returns>A Result indicating success or failure.</returns>
    Result ValidateMaxLength(Person person);

    /// <summary>
    ///     Validates the required properties of the provided user.
    /// </summary>
    /// <param name="person">The user to validate.</param>
    /// <returns>A Result indicating success or failure.</returns>
    Result ValidateRequiredProperties(Person person);

    /// <summary>
    ///     Validates that the provided username does not already exist.
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result indicating success or failure.</returns>
    Task<Result> ValidatePersonnameDoesNotExistAsync(string username, CancellationToken cancellationToken);
}
