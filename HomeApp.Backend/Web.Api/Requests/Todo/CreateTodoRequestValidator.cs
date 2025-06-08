using FluentValidation;

namespace Web.Api.Requests.Todo;

public sealed class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150);

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority.");
    }
}
