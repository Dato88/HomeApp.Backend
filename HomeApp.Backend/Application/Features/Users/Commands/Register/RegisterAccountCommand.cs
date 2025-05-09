﻿using Application.Abstractions.Messaging;
using Domain.Entities.User;

namespace Application.Features.Users.Commands.Register;

public sealed record RegisterAccountCommand(
    string Email,
    string? FirstName,
    string? LastName,
    string Password,
    string? ConfirmPassword,
    string? ClientUri)
    : ICommand<Guid>
{
    public static implicit operator User(RegisterAccountCommand item) =>
        new() { FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, UserName = item.Email };
}
