using HomeApp.DataAccess.Cruds.Interfaces.People;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.People.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Facades;

public class PersonFacade(
    IPersonCommands personCommands,
    IPersonQueries personQueries,
    IHttpContextAccessor httpContextAccessor,
    ILogger<PersonFacade> logger) : LoggerExtension<PersonFacade>(logger), IPersonFacade
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IPersonCommands _personCommands = personCommands;
    private readonly IPersonQueries _personQueries = personQueries;

    public async Task<PersonDto> GetUserPersonAsync(CancellationToken cancellationToken)
    {
        try
        {
            var email = _httpContextAccessor.HttpContext.User.Identity.Name;

            return await _personQueries.FindByEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Get person failed: {ex}", DateTime.Now);

            return null;
        }
    }

    public async Task<PersonDto> GetPersonByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _personQueries.FindByEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Get person failed: {ex}", DateTime.Now);

            return null;
        }
    }

    public async Task CreatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        try
        {
            await _personCommands.CreateAsync(person, cancellationToken);

            LogInformation($"Creating person: {person}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Creating person failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task UpdatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        try
        {
            await _personCommands.UpdateAsync(person, cancellationToken);

            LogInformation($"Updating person: {person}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Updating person failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task DeletePersonAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _personCommands.DeleteAsync(id, cancellationToken);

            LogInformation($"Deleting person: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting person failed: {ex.Message}", DateTime.Now);
        }
    }
}
