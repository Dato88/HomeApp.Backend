using Application.Abstractions.Authentication;
using Application.Abstractions.HouseholdModule;
using Domain.Entities.Households;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Households.Commands;

public sealed class HouseholdCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : IHouseholdCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateHouseholdAsync(string name, CancellationToken cancellationToken)
    {
        var household = new Household { Name = name, CreatedById = _executionContext.PersonId };
        household.Members.Add(new HouseholdMember
        {
            PersonId = _executionContext.PersonId, CreatedById = _executionContext.PersonId
        });

        _dbContext.Households.Add(household);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(household.HouseholdId);
    }

    public async Task<Result<int>> UpdateHouseholdAsync(int householdId, string name,
        CancellationToken cancellationToken)
    {
        var household = await _dbContext.Households.SingleOrDefaultAsync(x =>
            x.HouseholdId == householdId &&
            x.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (household == null)
            return Result.Failure<int>(HouseholdErrors.UpdateFailedWithMessage("HouseholdId is invalid"));

        household.Name = name;
        household.UpdatedById = _executionContext.PersonId;
        household.UpdatedAt = DateTime.UtcNow;

        _dbContext.Households.Update(household);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(household.HouseholdId);
    }

    public async Task<Result<int>> DeleteHouseholdAsync(int householdId, CancellationToken cancellationToken)
    {
        var household = await _dbContext.Households.SingleOrDefaultAsync(x =>
            x.HouseholdId == householdId &&
            x.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (household == null)
            return Result.Failure<int>(HouseholdErrors.DeleteFailedWithMessage("HouseholdId is invalid"));

        _dbContext.Households.Remove(household);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(householdId);
    }

    public async Task<Result<int>> AddHouseholdMemberAsync(int householdId, string? email, int? personId,
        CancellationToken cancellationToken)
    {
        var callerIsMember = _dbContext.HouseholdMembers.Any(x =>
            x.HouseholdId == householdId && x.PersonId == _executionContext.PersonId);

        if (!callerIsMember)
            return Result.Failure<int>(HouseholdErrors.AddMemberFailedWithMessage("HouseholdId is invalid"));

        var person = personId.HasValue
            ? await _dbContext.People.SingleOrDefaultAsync(p => p.PersonId == personId.Value, cancellationToken)
            : await _dbContext.People.SingleOrDefaultAsync(p => p.Email == email, cancellationToken);

        if (person == null)
            return Result.Failure<int>(HouseholdErrors.AddMemberFailedWithMessage("Person was not found"));

        var alreadyMember = _dbContext.HouseholdMembers.Any(x =>
            x.HouseholdId == householdId && x.PersonId == person.PersonId);

        if (alreadyMember)
            return Result.Failure<int>(HouseholdErrors.AddMemberFailedWithMessage("Person is already a member"));

        var member = new HouseholdMember
        {
            HouseholdId = householdId, PersonId = person.PersonId, CreatedById = _executionContext.PersonId
        };

        _dbContext.HouseholdMembers.Add(member);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(member.HouseholdMemberId);
    }

    public async Task<Result<int>> RemoveHouseholdMemberAsync(int householdId, int personId,
        CancellationToken cancellationToken)
    {
        var callerIsMember = _dbContext.HouseholdMembers.Any(x =>
            x.HouseholdId == householdId && x.PersonId == _executionContext.PersonId);

        if (!callerIsMember)
            return Result.Failure<int>(HouseholdErrors.RemoveMemberFailedWithMessage("HouseholdId is invalid"));

        var member = await _dbContext.HouseholdMembers.SingleOrDefaultAsync(x =>
            x.HouseholdId == householdId && x.PersonId == personId, cancellationToken);

        if (member == null)
            return Result.Failure<int>(HouseholdErrors.RemoveMemberFailedWithMessage("Person is not a member"));

        var memberCount = await _dbContext.HouseholdMembers.CountAsync(x =>
            x.HouseholdId == householdId, cancellationToken);

        if (memberCount <= 1)
            return Result.Failure<int>(
                HouseholdErrors.RemoveMemberFailedWithMessage("The last member cannot be removed"));

        _dbContext.HouseholdMembers.Remove(member);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(member.HouseholdMemberId);
    }
}
