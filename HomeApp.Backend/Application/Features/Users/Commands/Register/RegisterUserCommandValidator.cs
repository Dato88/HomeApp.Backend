using FluentValidation;

namespace Application.Features.Users.Commands.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8).WithMessage("Password is required");
        RuleFor(c => c.ConfirmPassword).NotEmpty().MinimumLength(8).WithMessage("ConfirmPassword is required");
        RuleFor(c => c.ConfirmPassword)
            .Equal(c => c.Password)
            .WithMessage("Passwords do not match.");
    }
}
