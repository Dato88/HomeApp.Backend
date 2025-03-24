﻿using FluentValidation;

namespace Application.Users.Commands.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);
        RuleFor(c => c.ConfirmPassword).NotEmpty().MinimumLength(8);
        RuleFor(c => c.ConfirmPassword)
            .Equal(c => c.Password)
            .WithMessage("Die Passwörter stimmen nicht überein.");
    }
}
