using HomeApp.Library.Cruds;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using HomeApp.Library.Models.Data_Transfer_Objects.PersonDtos;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Facades;

public class PersonFacade(
    IPersonCrud personCrud,
    ILogger<BudgetFacade> logger) : BudgetLoggerExtension<BudgetFacade>(logger), IPersonFacade
{
    private readonly IPersonCrud _personCrud = personCrud;

    public async Task<IEnumerable<PersonDto>> GetPeopleAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _personCrud.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            LogException($"Get people failed: {ex}", DateTime.Now);

            return null;
        }
    }

    public async Task CreatePersonAsync(Person person, CancellationToken cancellationToken)
    {
        try
        {
            await _personCrud.CreateAsync(person, cancellationToken);

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
            await _personCrud.UpdateAsync(person, cancellationToken);

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
            await _personCrud.DeleteAsync(id, cancellationToken);

            LogInformation($"Deleting person: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting person failed: {ex.Message}", DateTime.Now);
        }
    }
}
