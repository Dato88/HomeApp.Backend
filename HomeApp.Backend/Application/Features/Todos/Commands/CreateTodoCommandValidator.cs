using FluentValidation;

namespace Application.Features.Todos.Commands;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(150)
            .WithMessage("Todo title is required");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid priority.");
    }
}
