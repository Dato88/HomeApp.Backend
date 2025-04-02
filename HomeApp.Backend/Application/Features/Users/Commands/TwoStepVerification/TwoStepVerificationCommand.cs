using Application.Abstractions.Messaging;

namespace Application.Users.Commands.TwoStepVerification;

public sealed record TwoStepVerificationCommand(
    string Email,
    string Provider,
    string Token) : ICommand<string>;
