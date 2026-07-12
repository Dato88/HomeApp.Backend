using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Commands;

public sealed class UpdateCategoryGroupCommandHandler(
    ICategoryGroupCommands categoryGroupCommands,
    IAppLogger<UpdateCategoryGroupCommandHandler> logger)
    : IRequestHandler<UpdateCategoryGroupCommand, Result<int>>
{
    private readonly ICategoryGroupCommands _categoryGroupCommands = categoryGroupCommands;
    private readonly IAppLogger<UpdateCategoryGroupCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateCategoryGroupCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryGroupCommands.UpdateCategoryGroupAsync(request.CategoryGroupId, request.Name,
            request.CategoryGroupType, request.TargetPercent, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating category group failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating category group: {result.Value}");

        return result;
    }
}
