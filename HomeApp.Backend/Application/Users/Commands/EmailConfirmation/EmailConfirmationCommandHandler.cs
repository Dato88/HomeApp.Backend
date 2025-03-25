using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Commands.EmailConfirmation;

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
            logger.LogWarning("Email is not found");

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var confirmResult = await userManager.ConfirmEmailAsync(user, command.Token);

        if (!confirmResult.Succeeded)
        {
            logger.LogWarning("Invalid Email Confirmation Request");

            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        return Result.Success(new Guid(user.Id));
    }
}
