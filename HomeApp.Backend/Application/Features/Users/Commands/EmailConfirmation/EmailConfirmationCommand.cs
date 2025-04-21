using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.EmailConfirmation;

public sealed record EmailConfirmationCommand(
    string Email,
    string Token)
    : ICommand<Guid>
{
}
