using FluentValidation;

namespace Application.Features.Users.Commands.EmailConfirmation;

internal sealed class EmailConfirmationCommandValidator : AbstractValidator<EmailConfirmationCommand>
{
    public EmailConfirmationCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.Token).NotEmpty().WithMessage("Token is required");
    }
}
