using Application.Abstractions.Data;
using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.People.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.People;

public class CommonPersonQueries(
    IHomeAppContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IAppLogger<CommonPersonQueries> logger) : ICommonPersonQueries
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<PersonDto?> GetUserPersonAsync(CancellationToken cancellationToken)
    {
        try
        {
            var email = _httpContextAccessor.HttpContext.User.Identity.Name;

            var person = await dbContext.People.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            return person;
        }
        catch (Exception ex)
        {
            logger.LogCritical($"Get person failed: {ex}");

            return null;
        }
    }

    public async Task<PersonDto?> GetPersonByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var person = await dbContext.People.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            return person;
        }
        catch (Exception ex)
        {
            logger.LogCritical($"Get person failed: {ex}");

            return null;
        }
    }
}
