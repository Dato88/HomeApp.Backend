using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.TwoStepVerification;

public sealed record TwoStepVerificationCommand(
    string Email,
    string Provider,
    string Token) : ICommand<string>;
