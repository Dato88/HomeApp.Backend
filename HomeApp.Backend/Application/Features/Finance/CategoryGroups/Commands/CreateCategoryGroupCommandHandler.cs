using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Commands;

public sealed class CreateCategoryGroupCommandHandler(
    ICategoryGroupCommands categoryGroupCommands,
    IAppLogger<CreateCategoryGroupCommandHandler> logger)
    : IRequestHandler<CreateCategoryGroupCommand, Result<int>>
{
    private readonly ICategoryGroupCommands _categoryGroupCommands = categoryGroupCommands;
    private readonly IAppLogger<CreateCategoryGroupCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateCategoryGroupCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryGroupCommands.CreateCategoryGroupAsync((CategoryGroup)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating category group failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating category group: {result.Value}");

        return result;
    }
}
