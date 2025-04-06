using System.Net.Mail;
using Application.Features.People.Validations;
using Domain.Entities.People;
using Domain.PredefinedMessages;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.People.Validations;

public class PersonValidation(HomeAppContext dbContext) : BaseContext(dbContext), IPersonValidation
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

    public async Task<Result> ValidatePersonnameDoesNotExistAsync(string username, CancellationToken cancellationToken)
    {
        var exists = await DbContext.People.AnyAsync(a => a.Username == username, cancellationToken);
        return exists
            ? Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.PersonAlreadyExists))
            : Result.Success();
    }

    public Result ValidateEmailFormat(string email) =>
        IsValidEmail(email)
            ? Result.Success()
            : Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.InvalidEmail));

    public Result ValidateRequiredProperties(Person person) =>
        string.IsNullOrWhiteSpace(person.FirstName) ||
        string.IsNullOrWhiteSpace(person.LastName) ||
        string.IsNullOrWhiteSpace(person.Email) ||
        string.IsNullOrWhiteSpace(person.UserId)
            ? Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.PropertiesMissing))
            : Result.Success();

    public Result ValidateMaxLength(Person person) =>
        person.Username.Length > 150 ||
        person.FirstName.Length > 150 ||
        person.LastName.Length > 150 ||
        person.Email.Length > 150 ||
        person.UserId.Length < 36
            ? Result.Failure(PersonErrors.CreateFailedWithMessage(PersonMessage.MaxLengthExeed))
            : Result.Success();
}
