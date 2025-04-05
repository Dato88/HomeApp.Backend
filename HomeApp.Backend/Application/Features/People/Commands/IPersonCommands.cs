using Domain.Entities.People;
using SharedKernel;

namespace Application.Features.People.Commands;

public interface IPersonCommands
{
    Task<Result<int>> CreatePersonAsync(Person person, CancellationToken cancellationToken);
    Task<Result> UpdatePersonAsync(Person person, CancellationToken cancellationToken);
    Task<Result> DeletePersonAsync(int id, CancellationToken cancellationToken);
}
