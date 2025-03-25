using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    UserManager<User> userManager,
    IAppLogger<ResetPasswordCommandHandler> logger)
    : ICommandHandler<ResetPasswordCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            logger.LogWarning("Email is not found");

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var resetPassResult =
            await userManager.ResetPasswordAsync(user, command.Token, command.Password);

        if (!resetPassResult.Succeeded)
        {
            logger.LogWarning("Reset Password failed");

            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        await userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        logger.LogInformation($"Password reset by {user.Id}");

        return Result.Success(new Guid(user.Id));
    }
}
