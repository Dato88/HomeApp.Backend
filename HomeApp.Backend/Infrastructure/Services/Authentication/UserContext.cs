using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using SharedKernel.ValueObjects;

namespace Infrastructure.Services.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public PersonId PersonId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetPersonId() ??
        throw new ApplicationException("User context is unavailable");

    public UserEmail UserEmail =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserEmail() ??
        throw new ApplicationException("User context is unavailable");

    public UserId UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");
}
