﻿using System.Net.Mail;
using HomeApp.DataAccess.Cruds;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Validations;

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

    public async Task ValidatePersonnameDoesNotExistAsync(string username, CancellationToken cancellationToken)
    {
        if (await DbContext.People.AnyAsync(a => a.Username == username, cancellationToken))
            throw new InvalidOperationException(PersonMessage.PersonAlreadyExists);
    }

    public void ValidateEmailFormat(string email)
    {
        if (!IsValidEmail(email))
            throw new ValidationException(PersonMessage.InvalidEmail);
    }

    public void ValidateRequiredProperties(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.FirstName) ||
            string.IsNullOrWhiteSpace(person.LastName) ||
            string.IsNullOrWhiteSpace(person.Email) ||
            string.IsNullOrWhiteSpace(person.UserId))
            throw new ValidationException(PersonMessage.PropertiesMissing);
    }

    public void ValidateMaxLength(Person person)
    {
        if (person.Username.Length > 150 ||
            person.FirstName.Length > 150 ||
            person.LastName.Length > 150 ||
            person.Email.Length > 150 ||
            person.UserId.Length < 36)
            throw new ValidationException(PersonMessage.MaxLengthExeed);
    }
}
