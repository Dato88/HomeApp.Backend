using HomeApp.DataAccess.Models.Data_Transfer_Objects.PersonDtos;

namespace HomeApp.Library.Facades.Interfaces;

public interface IPersonFacade
{
    Task<IEnumerable<PersonDto>> GetPeopleAsync(CancellationToken cancellationToken);
    Task<PersonDto> GetUserPersonAsync(CancellationToken cancellationToken);
    Task<PersonDto> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
    Task CreatePersonAsync(Person person, CancellationToken cancellationToken);
    Task UpdatePersonAsync(Person person, CancellationToken cancellationToken);
    Task DeletePersonAsync(int id, CancellationToken cancellationToken);
}
