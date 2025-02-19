﻿using HomeApp.Library.Common.Interfaces.People;
using HomeApp.Library.People.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Common.People;

public class CommonPersonQueries(
    HomeAppContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    ILogger<CommonPersonQueries> logger) : LoggerExtension<CommonPersonQueries>(logger), ICommonPersonQueries
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
            LogException($"Get person failed: {ex}", DateTime.Now);

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
            LogException($"Get person failed: {ex}", DateTime.Now);

            return null;
        }
    }
}
