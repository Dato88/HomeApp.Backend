using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Commands.TwoStepVerification;

internal sealed class TwoStepVerificationCommandHandler(UserManager<User> userManager, ITokenProvider tokenProvider)
    : ICommandHandler<TwoStepVerificationCommand, string>
{
    public async Task<Result<string>> Handle(TwoStepVerificationCommand query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(query.Email);

        if (user is null) return Result.Failure<string>(UserErrors.NotFoundByEmail);

        var validVerification =
            await userManager.VerifyTwoFactorTokenAsync(user, query.Provider, query.Token);

        if (!validVerification)
            return Result.Failure<string>(UserErrors.Unauthorized());

        var token = tokenProvider.Create(user);

        return Result.Success(token);
    }
}
