using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Commands;

public sealed class DeleteCategoryGroupCommandHandler(
    ICategoryGroupCommands categoryGroupCommands,
    IAppLogger<DeleteCategoryGroupCommandHandler> logger)
    : IRequestHandler<DeleteCategoryGroupCommand, Result<int>>
{
    private readonly ICategoryGroupCommands _categoryGroupCommands = categoryGroupCommands;
    private readonly IAppLogger<DeleteCategoryGroupCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteCategoryGroupCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryGroupCommands.DeleteCategoryGroupAsync(request.CategoryGroupId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting category group failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting category group: {result.Value}");

        return result;
    }
}
