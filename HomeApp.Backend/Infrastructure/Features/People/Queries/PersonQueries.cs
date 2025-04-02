using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Logging;
using Application.Features.People.Dtos;
using Application.Features.People.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Features.People.Queries;

public class PersonQueries(
    IHomeAppContext dbContext,
    IUserContext userContext,
    IAppLogger<PersonQueries> logger) : IPersonQueries
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
