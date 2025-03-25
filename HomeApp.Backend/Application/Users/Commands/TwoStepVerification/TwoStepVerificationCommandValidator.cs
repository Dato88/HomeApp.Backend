using FluentValidation;

namespace Application.Users.Commands.TwoStepVerification;

public class TwoStepVerificationCommandValidator : AbstractValidator<TwoStepVerificationCommand>
{
    public TwoStepVerificationCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.Provider).NotEmpty().WithMessage("Provider is required");
        RuleFor(c => c.Token).NotEmpty().WithMessage("Token is required");
    }
}
