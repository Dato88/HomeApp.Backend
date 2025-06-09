using FluentValidation;

namespace Application.Features.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.ClientUri).NotEmpty().WithMessage("ClientUri is required");
    }
}
