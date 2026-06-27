using Application.Abstractions.Authentication;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public int PersonId
    {
        get
        {
            if (httpContextAccessor.HttpContext?.Items.TryGetValue(
                    DependencyInjection.PersonIdItemKey,
                    out var value) == true
                && value is int personId)
                return personId;

            return httpContextAccessor.HttpContext?.User.GetPersonId()
                   ?? throw new ApplicationException("User context is unavailable");
        }
    }

    public UserEmail UserEmail =>
        httpContextAccessor.HttpContext?.User.GetUserEmail()
        ?? throw new ApplicationException("User context is unavailable");

    public Guid UserId =>
        httpContextAccessor.HttpContext?.User.GetUserId()
        ?? throw new ApplicationException("User context is unavailable");
}
