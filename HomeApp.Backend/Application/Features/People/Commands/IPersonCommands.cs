using Domain.Entities.People;

namespace Application.Features.People.Commands;

public interface IPersonCommands
{
    Task<int> CreatePersonAsync(Person person, CancellationToken cancellationToken);
    Task<bool> UpdatePersonAsync(Person person, CancellationToken cancellationToken);
    Task DeletePersonAsync(int id, CancellationToken cancellationToken);
}
