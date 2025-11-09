using FluentValidation;

namespace Application.Features.Todos.Commands;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(x => x.TodoId)
            .GreaterThan(0).WithMessage("The todo ID must be greater than zero.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("The title must not be empty.")
            .MaximumLength(150).WithMessage("The title must not exceed 150 characters.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value.");
    }
}
