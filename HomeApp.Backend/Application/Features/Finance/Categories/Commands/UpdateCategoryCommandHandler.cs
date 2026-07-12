using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Commands;

public sealed class UpdateCategoryCommandHandler(
    ICategoryCommands categoryCommands,
    IAppLogger<UpdateCategoryCommandHandler> logger)
    : IRequestHandler<UpdateCategoryCommand, Result<int>>
{
    private readonly ICategoryCommands _categoryCommands = categoryCommands;
    private readonly IAppLogger<UpdateCategoryCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryCommands.UpdateCategoryAsync(request.CategoryId, request.Name,
            request.CategoryType, request.CategoryGroupId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating category failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating category: {result.Value}");

        return result;
    }
}
