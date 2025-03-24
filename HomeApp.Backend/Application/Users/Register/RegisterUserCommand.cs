using Application.DTOs.Register;
using Domain.Entities.User;
using MediatR;

namespace Application.Users.Register;

public sealed record RegisterUserCommand(
    string Email,
    string? FirstName,
    string? LastName,
    string Password,
    string? ConfirmPassword,
    string? ClientUri)
    : IRequest<RegistrationResponseDto>
{
    public static implicit operator RegisterUserDto(RegisterUserCommand item) => new()
    {
        FirstName = item.FirstName,
        LastName = item.LastName,
        Email = item.Email,
        Password = item.Password,
        ConfirmPassword = item.ConfirmPassword
    };

    public static implicit operator User(RegisterUserCommand item) =>
        new() { FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, UserName = item.Email };
}
