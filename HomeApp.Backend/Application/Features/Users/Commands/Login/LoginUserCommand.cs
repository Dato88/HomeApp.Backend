using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.Login;

public sealed record LoginUserCommand(string Email, string Password, string? ClientUri) : ICommand<AuthResponse>;
