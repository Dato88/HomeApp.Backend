using FluentValidation;

namespace Application.Users.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8).WithMessage("Password is required");
        RuleFor(c => c.ConfirmPassword).NotEmpty().MinimumLength(8).WithMessage("ConfirmPassword is required");
        RuleFor(c => c.ConfirmPassword)
            .Equal(c => c.Password)
            .WithMessage("Passwords do not match.");
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.Token).NotEmpty().WithMessage("Token is required");
    }
}
