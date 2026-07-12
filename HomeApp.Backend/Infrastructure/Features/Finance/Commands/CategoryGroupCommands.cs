using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Commands;

public sealed class CategoryGroupCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ICategoryGroupCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateCategoryGroupAsync(CategoryGroup categoryGroup,
        CancellationToken cancellationToken)
    {
        var isMember = _dbContext.HouseholdMembers.Any(m =>
            m.HouseholdId == categoryGroup.HouseholdId && m.PersonId == _executionContext.PersonId);

        if (!isMember)
            return Result.Failure<int>(FinanceErrors.CategoryGroupCreateFailedWithMessage("HouseholdId is invalid"));

        var nameExists = _dbContext.CategoryGroups.Any(g =>
            g.HouseholdId == categoryGroup.HouseholdId && g.Name == categoryGroup.Name);

        if (nameExists)
            return Result.Failure<int>(FinanceErrors.CategoryGroupCreateFailedWithMessage("Name already exists"));

        categoryGroup.CreatedById = _executionContext.PersonId;

        _dbContext.CategoryGroups.Add(categoryGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(categoryGroup.CategoryGroupId);
    }

    public async Task<Result<int>> UpdateCategoryGroupAsync(int categoryGroupId, string name,
        CategoryType categoryGroupType, decimal? targetPercent, CancellationToken cancellationToken)
    {
        var categoryGroup = await _dbContext.CategoryGroups.SingleOrDefaultAsync(g =>
            g.CategoryGroupId == categoryGroupId &&
            g.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (categoryGroup == null)
            return Result.Failure<int>(
                FinanceErrors.CategoryGroupUpdateFailedWithMessage("CategoryGroupId is invalid"));

        var nameExists = _dbContext.CategoryGroups.Any(g =>
            g.HouseholdId == categoryGroup.HouseholdId && g.Name == name && g.CategoryGroupId != categoryGroupId);

        if (nameExists)
            return Result.Failure<int>(FinanceErrors.CategoryGroupUpdateFailedWithMessage("Name already exists"));

        categoryGroup.Name = name;
        categoryGroup.CategoryGroupType = categoryGroupType;
        categoryGroup.TargetPercent = targetPercent;
        categoryGroup.UpdatedById = _executionContext.PersonId;
        categoryGroup.UpdatedAt = DateTime.UtcNow;

        _dbContext.CategoryGroups.Update(categoryGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(categoryGroup.CategoryGroupId);
    }

    public async Task<Result<int>> DeleteCategoryGroupAsync(int categoryGroupId, CancellationToken cancellationToken)
    {
        var categoryGroup = await _dbContext.CategoryGroups.SingleOrDefaultAsync(g =>
            g.CategoryGroupId == categoryGroupId &&
            g.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (categoryGroup == null)
            return Result.Failure<int>(
                FinanceErrors.CategoryGroupDeleteFailedWithMessage("CategoryGroupId is invalid"));

        // Categories keep existing; their category_group_id FK is set to null by the database
        _dbContext.CategoryGroups.Remove(categoryGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(categoryGroupId);
    }
}
