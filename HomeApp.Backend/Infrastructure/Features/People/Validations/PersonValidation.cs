using System.Net.Mail;
using Application.Features.People.Validations;
using Domain.Entities.People;
using Domain.PredefinedMessages;
using Domain.ValueObjects;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.People.Validations;

internal sealed class PersonValidation(HomeAppContext dbContext) : BaseContext(dbContext), IPersonValidation
{
    public bool IsValidEmail(UserEmail email)
    {
        try
        {
            MailAddress addr = new(email.Value);
            return addr.Address == email.Value;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Result> ValidatePersonnameDoesNotExistAsync(string username, CancellationToken cancellationToken)
    {
        var exists = await DbContext.People.AnyAsync(a => a.Username == username, cancellationToken);
        return exists
            ? Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.PersonAlreadyExists))
            : Result.Success();
    }

    public Result ValidateRequiredProperties(Person person) =>
        string.IsNullOrWhiteSpace(person.FirstName) ||
        string.IsNullOrWhiteSpace(person.LastName) ||
        string.IsNullOrWhiteSpace(person.Email.Value) ||
        string.IsNullOrWhiteSpace(person.UserId)
            ? Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.PropertiesMissing))
            : Result.Success();

    public Result ValidateMaxLength(Person person) =>
        person.Username.Length > 150 ||
        person.FirstName.Length > 150 ||
        person.LastName.Length > 150 ||
        person.Email.Value.Length > 150 ||
        person.UserId.Length < 36
            ? Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.MaxLengthExeed))
            : Result.Success();

    public Result ValidateEmailFormat(UserEmail email) =>
        IsValidEmail(email)
            ? Result.Success()
            : Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.InvalidEmail));
}
