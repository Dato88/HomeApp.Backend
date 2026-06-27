using Application.Abstractions.Authentication;
using Domain.ValueObjects;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Authentication;

internal sealed class DevUserContext(IOptions<DevUserContextOptions> options) : IUserContext
{
    private readonly DevUserContextOptions _options = options.Value;

    public int PersonId => _options.PersonId;

    public UserEmail UserEmail => new(_options.Email);

    public Guid UserId => _options.UserId;
}
