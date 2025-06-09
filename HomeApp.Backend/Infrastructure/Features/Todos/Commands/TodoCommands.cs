using Application.Features.Todos.Commands;
using Domain.Entities.Todos;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Features.Todos.Commands;

public sealed class TodoCommands(HomeAppContext dbContext) : BaseCommands<Todo>(dbContext), ITodoCommands
{
    public override async Task<Result<int>> CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        if (todo is null)
            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage("Todo is null"));

        if (todo.Priority < 0)
            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage("Priority is invalid"));

        if (!todo.TodoPeople.Any(x => x.PersonId > 0))
            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage("Todo must have at least one valid person"));

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(todo.Id);
    }

    public override async Task<Result> UpdateAsync(Todo todo, CancellationToken cancellationToken)
    {
        if (todo is null)
            return Result.Failure(TodoErrors.UpdateFailedWithMessage("Todo is null"));

        if (todo.Priority < 0)
            return Result.Failure(TodoErrors.UpdateFailedWithMessage("Priority is invalid"));

        var existingTodo = await DbContext.Todos.FindAsync(todo.Id, cancellationToken);

        if (existingTodo == null)
            return Result.Failure(TodoErrors.UpdateFailed(todo.Id));

        existingTodo.Name = todo.Name;
        existingTodo.Done = todo.Done;
        existingTodo.Priority = todo.Priority;
        existingTodo.LastModified = DateTime.UtcNow;

        DbContext.Todos.Update(existingTodo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public override async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return Result.Failure(TodoErrors.DeleteFailed(id));

        var todo = await DbContext.Todos.FindAsync(new object[] { id }, cancellationToken);

        if (todo == null)
            return Result.Failure(TodoErrors.DeleteFailed(id));

        DbContext.Todos.Remove(todo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
