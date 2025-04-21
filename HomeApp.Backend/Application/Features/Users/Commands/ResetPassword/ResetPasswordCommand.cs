using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Password,
    string ConfirmPassword,
    string Email,
    string Token)
    : ICommand<Guid>
{
}
