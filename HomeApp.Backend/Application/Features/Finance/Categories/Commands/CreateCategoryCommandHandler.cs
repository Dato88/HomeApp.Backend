using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Commands;

public sealed class CreateCategoryCommandHandler(
    ICategoryCommands categoryCommands,
    IAppLogger<CreateCategoryCommandHandler> logger)
    : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    private readonly ICategoryCommands _categoryCommands = categoryCommands;
    private readonly IAppLogger<CreateCategoryCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryCommands.CreateCategoryAsync((Category)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating category failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating category: {result.Value}");

        return result;
    }
}
