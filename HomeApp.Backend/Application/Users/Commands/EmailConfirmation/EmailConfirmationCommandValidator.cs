using FluentValidation;

namespace Application.Users.Commands.EmailConfirmation;

public class EmailConfirmationCommandValidator : AbstractValidator<EmailConfirmationCommand>
{
    public EmailConfirmationCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.Token).NotEmpty().WithMessage("Token is required");
    }
}
