using Application.DTOs.Authentication;
using MediatR;

namespace Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password, string? ClientUri) : IRequest<AuthResponseDto>;
