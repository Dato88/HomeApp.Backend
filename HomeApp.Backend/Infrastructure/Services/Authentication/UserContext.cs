using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public int PersonId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetPersonId() ??
        throw new ApplicationException("User context is unavailable");

    public string UserEmail =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserEmail() ??
        throw new ApplicationException("User context is unavailable");

    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");
}
