using Application.Features.Todos.Commands;
using Domain.Entities.Todos;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Features.Todos.Commands;

public sealed class TodoCommands(HomeAppContext dbContext) : ITodoCommands
{
    public async Task<Result> UpdateAsync(Todo todo, CancellationToken cancellationToken)
    {
        if (todo is null)
            return Result.Failure(TodoErrors.UpdateFailedWithMessage("Todo is null"));

        if (todo.Priority < 0)
            return Result.Failure(TodoErrors.UpdateFailedWithMessage("Priority is invalid"));

        var existingTodo = await dbContext.Todos.FindAsync(todo.TodoId, cancellationToken);

        if (existingTodo == null)
            return Result.Failure(TodoErrors.UpdateFailed(todo.TodoId));

        existingTodo.Title = todo.Title;
        existingTodo.Done = todo.Done;
        existingTodo.Priority = todo.Priority;
        existingTodo.UpdatedAt = DateTime.UtcNow;

        dbContext.Todos.Update(existingTodo);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<int>> CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        if (todo is null)
            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage("Todo is null"));

        if (todo.Priority < 0)
            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage("Priority is invalid"));

        if (!todo.TodoPeople.Any(x => x.PersonId != null))
            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage("Todo must have at least one valid person"));

        dbContext.Todos.Add(todo);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(todo.TodoId);
    }

    public async Task<Result> DeleteAsync(int todoId, CancellationToken cancellationToken)
    {
        if (todoId <= 0)
            return Result.Failure(TodoErrors.DeleteFailed(todoId));

        var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);

        if (todo == null)
            return Result.Failure(TodoErrors.DeleteFailed(todoId));

        dbContext.Todos.Remove(todo);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
