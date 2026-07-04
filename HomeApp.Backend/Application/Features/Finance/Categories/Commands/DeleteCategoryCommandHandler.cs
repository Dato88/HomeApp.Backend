using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Commands;

public sealed class DeleteCategoryCommandHandler(
    ICategoryCommands categoryCommands,
    IAppLogger<DeleteCategoryCommandHandler> logger)
    : IRequestHandler<DeleteCategoryCommand, Result<int>>
{
    private readonly ICategoryCommands _categoryCommands = categoryCommands;
    private readonly IAppLogger<DeleteCategoryCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryCommands.DeleteCategoryAsync(request.CategoryId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting category failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting category: {result.Value}");

        return result;
    }
}
