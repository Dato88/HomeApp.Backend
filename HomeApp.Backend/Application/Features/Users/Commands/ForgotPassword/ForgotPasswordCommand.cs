using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string Email,
    string ClientUri)
    : ICommand<Guid>
{
}
