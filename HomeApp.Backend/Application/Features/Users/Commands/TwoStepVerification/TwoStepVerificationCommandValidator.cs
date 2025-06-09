using FluentValidation;

namespace Application.Features.Users.Commands.TwoStepVerification;

internal sealed class TwoStepVerificationCommandValidator : AbstractValidator<TwoStepVerificationCommand>
{
    public TwoStepVerificationCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.Provider).NotEmpty().WithMessage("Provider is required");
        RuleFor(c => c.Token).NotEmpty().WithMessage("Token is required");
    }
}
