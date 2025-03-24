using Application.Abstractions.Messaging;
using Application.DTOs.Authentication;

namespace Application.Users.Commands.Login;

public sealed record LoginUserCommand(string Email, string Password, string? ClientUri) : ICommand<AuthResponseDto>;
