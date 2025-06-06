using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Features.Users.Commands.TwoStepVerification;

internal sealed class TwoStepVerificationCommandHandler(
    UserManager<User> userManager,
    ITokenProvider tokenProvider,
    IAppLogger<TwoStepVerificationCommandHandler> logger)
    : ICommandHandler<TwoStepVerificationCommand, string>
{
    public async Task<Result<string>> Handle(TwoStepVerificationCommand query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(query.Email);

        if (user is null)
        {
            logger.LogWarning($"2FA attempt failed – no user found with email: {query.Email}");
            return Result.Failure<string>(UserErrors.NotFoundByEmail);
        }

        var validVerification = await userManager.VerifyTwoFactorTokenAsync(user, query.Provider, query.Token);

        if (!validVerification)
        {
            logger.LogWarning(
                $"Invalid 2FA token for user {query.Email} (UserId: {user.Id}) via provider {query.Provider}");
            return Result.Failure<string>(UserErrors.Unauthorized());
        }

        var token = await tokenProvider.Create(user);

        logger.LogInformation($"2FA succeeded for user {query.Email} (UserId: {user.Id}). Token issued.");

        return Result.Success(token);
    }
}
