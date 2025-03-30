using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.People.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.People;

public class CommonPersonQueries(
    IHomeAppContext dbContext,
    IUserContext userContext,
    IAppLogger<CommonPersonQueries> logger) : ICommonPersonQueries
{
    public async Task<PersonDto?> GetUserPersonAsync(CancellationToken cancellationToken)
    {
        try
        {
            var userId = userContext.UserId.ToString();

            var person = await dbContext.People.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            return person;
        }
        catch (Exception ex)
        {
            logger.LogError($"Get person failed: {ex}");

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
            logger.LogError($"Get person failed: {ex}");

            return null;
        }
    }
}
