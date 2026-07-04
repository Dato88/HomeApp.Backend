using FluentValidation;

namespace Application.Features.Finance.Categories.Commands;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150);
    }
}
