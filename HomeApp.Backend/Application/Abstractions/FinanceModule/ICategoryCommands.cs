using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ICategoryCommands
{
    Task<Result<int>> CreateCategoryAsync(Category category, CancellationToken cancellationToken);

    Task<Result<int>> UpdateCategoryAsync(int categoryId, string name, CategoryType categoryType,
        int? categoryGroupId, CancellationToken cancellationToken);

    Task<Result<int>> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
}
