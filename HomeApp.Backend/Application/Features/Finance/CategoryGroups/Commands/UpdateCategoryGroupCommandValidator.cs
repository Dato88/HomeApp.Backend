using Domain.Entities.Finance.Enums;
using FluentValidation;

namespace Application.Features.Finance.CategoryGroups.Commands;

internal sealed class UpdateCategoryGroupCommandValidator : AbstractValidator<UpdateCategoryGroupCommand>
{
    public UpdateCategoryGroupCommandValidator()
    {
        RuleFor(c => c.CategoryGroupId).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150);
        RuleFor(c => c.CategoryGroupType)
            .Must(t => t is CategoryType.Income or CategoryType.Expense)
            .WithMessage("CategoryGroupType must be Income or Expense.");
        RuleFor(c => c.TargetPercent)
            .InclusiveBetween(0, 100)
            .When(c => c.TargetPercent.HasValue);
    }
}
