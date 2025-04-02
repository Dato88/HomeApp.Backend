using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Features.Users.Commands.EmailConfirmation;

internal sealed class EmailConfirmationCommandHandler(
    UserManager<User> userManager,
    IAppLogger<EmailConfirmationCommandHandler> logger)
    : ICommandHandler<EmailConfirmationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(EmailConfirmationCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            logger.LogWarning($"Email confirmation attempt failed – no user found with email: {command.Email}");

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var confirmResult = await userManager.ConfirmEmailAsync(user, command.Token);

        if (!confirmResult.Succeeded)
        {
            logger.LogWarning(
                $"Email confirmation failed for {command.Email} (UserId: {user.Id}). Possibly invalid or expired token.");

            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        logger.LogInformation($"Email successfully confirmed for {command.Email} (UserId: {user.Id})");

        return Result.Success(new Guid(user.Id));
    }
}
