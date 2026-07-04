using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Commands;

public sealed class CategoryCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ICategoryCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        var isMember = _dbContext.HouseholdMembers.Any(m =>
            m.HouseholdId == category.HouseholdId && m.PersonId == _executionContext.PersonId);

        if (!isMember)
            return Result.Failure<int>(FinanceErrors.CategoryCreateFailedWithMessage("HouseholdId is invalid"));

        var nameExists = _dbContext.Categories.Any(c =>
            c.HouseholdId == category.HouseholdId && c.Name == category.Name);

        if (nameExists)
            return Result.Failure<int>(FinanceErrors.CategoryCreateFailedWithMessage("Name already exists"));

        category.CreatedById = _executionContext.PersonId;

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(category.CategoryId);
    }

    public async Task<Result<int>> UpdateCategoryAsync(int categoryId, string name, CategoryType categoryType,
        CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.SingleOrDefaultAsync(c =>
            c.CategoryId == categoryId &&
            c.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (category == null)
            return Result.Failure<int>(FinanceErrors.CategoryUpdateFailedWithMessage("CategoryId is invalid"));

        var nameExists = _dbContext.Categories.Any(c =>
            c.HouseholdId == category.HouseholdId && c.Name == name && c.CategoryId != categoryId);

        if (nameExists)
            return Result.Failure<int>(FinanceErrors.CategoryUpdateFailedWithMessage("Name already exists"));

        category.Name = name;
        category.CategoryType = categoryType;
        category.UpdatedById = _executionContext.PersonId;
        category.UpdatedAt = DateTime.UtcNow;

        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(category.CategoryId);
    }

    public async Task<Result<int>> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.SingleOrDefaultAsync(c =>
            c.CategoryId == categoryId &&
            c.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (category == null)
            return Result.Failure<int>(FinanceErrors.CategoryDeleteFailedWithMessage("CategoryId is invalid"));

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(categoryId);
    }
}
