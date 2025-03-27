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
            logger.LogWarning($"Password reset failed – no user found with email: {command.Email}");

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var resetPassResult = await userManager.ResetPasswordAsync(user, command.Token, command.Password);

        if (!resetPassResult.Succeeded)
        {
            var errors = string.Join(", ", resetPassResult.Errors.Select(e => e.Description));
            logger.LogWarning($"Password reset failed for {command.Email} (UserId: {user.Id}). Reason(s): {errors}");

            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        await userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        logger.LogInformation($"Password reset successful for {command.Email} (UserId: {user.Id})");

        return Result.Success(new Guid(user.Id));
    }
}
