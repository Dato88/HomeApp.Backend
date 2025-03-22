using Domain.People;

namespace Application.Common.Interfaces.People;

public interface ICommonPersonCommands
{
    Task<int> CreatePersonAsync(Person person, CancellationToken cancellationToken);
    Task<bool> UpdatePersonAsync(Person person, CancellationToken cancellationToken);
    Task DeletePersonAsync(int id, CancellationToken cancellationToken);
}
