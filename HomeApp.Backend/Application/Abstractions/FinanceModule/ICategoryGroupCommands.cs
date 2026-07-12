using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ICategoryGroupCommands
{
    Task<Result<int>> CreateCategoryGroupAsync(CategoryGroup categoryGroup, CancellationToken cancellationToken);

    Task<Result<int>> UpdateCategoryGroupAsync(int categoryGroupId, string name, CategoryType categoryGroupType,
        decimal? targetPercent, CancellationToken cancellationToken);

    Task<Result<int>> DeleteCategoryGroupAsync(int categoryGroupId, CancellationToken cancellationToken);
}
