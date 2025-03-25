using Application.Abstractions.Messaging;

namespace Application.Users.Commands.EmailConfirmation;

public sealed record EmailConfirmationCommand(
    string Email,
    string Token)
    : ICommand<Guid>
{
}
