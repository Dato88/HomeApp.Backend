using FluentValidation;

namespace Web.Api.Requests.Todo;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("The todo ID must be greater than zero.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The name must not be empty.")
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value.");
    }
}
