using FluentValidation;

namespace Application.Features.Todos.Commands;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Todo name is required");
    }
}
