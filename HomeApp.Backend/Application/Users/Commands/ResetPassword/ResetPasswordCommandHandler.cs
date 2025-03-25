using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    UserManager<User> userManager,
    ILogger<ResetPasswordCommandHandler> logger)
    : ICommandHandler<ResetPasswordCommand, Guid>
{
    private readonly LoggerExtension<ResetPasswordCommandHandler> _logger = new(logger);

    public async Task<Result<Guid>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            _logger.LogWarning("Email is not found", DateTime.Now);

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var resetPassResult =
            await userManager.ResetPasswordAsync(user, command.Token, command.Password);

        if (!resetPassResult.Succeeded)
        {
            _logger.LogWarning("Reset Password failed", DateTime.Now);

            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        await userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        _logger.LogInformation($"Password reset by {user.Id}", DateTime.Now);

        return Result.Success(new Guid(user.Id));
    }
}
