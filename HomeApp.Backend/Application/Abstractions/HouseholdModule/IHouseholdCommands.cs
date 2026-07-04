using SharedKernel;

namespace Application.Abstractions.HouseholdModule;

public interface IHouseholdCommands
{
    Task<Result<int>> CreateHouseholdAsync(string name, CancellationToken cancellationToken);
    Task<Result<int>> UpdateHouseholdAsync(int householdId, string name, CancellationToken cancellationToken);
    Task<Result<int>> DeleteHouseholdAsync(int householdId, CancellationToken cancellationToken);

    Task<Result<int>> AddHouseholdMemberAsync(int householdId, string? email, int? personId,
        CancellationToken cancellationToken);

    Task<Result<int>> RemoveHouseholdMemberAsync(int householdId, int personId, CancellationToken cancellationToken);
}
