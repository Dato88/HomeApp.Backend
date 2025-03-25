using Application.Abstractions.Data;
using Application.Common.Interfaces.Todos;
using Domain.Entities.Todos;
using Domain.PredefinedMessages;

namespace Application.Common.Todos;

public class TodoCommands(IHomeAppContext dbContext) : BaseCommands<Todo>(dbContext), ITodoCommands
{
    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(Todo.Id));

        var todo = await DbContext.Todos.FindAsync(new object[] { id }, cancellationToken);

        if (todo == null)
            throw new InvalidOperationException(TodoMessage.TodoNotFound);

        DbContext.Todos.Remove(todo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<int> CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todo);
        ArgumentOutOfRangeException.ThrowIfNegative((int)todo.Priority, nameof(todo.Priority));

        if (!todo.TodoPeople.Any(x => x.PersonId > 0))
            throw new InvalidOperationException("Todo can`t be created without personId.");

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return todo.Id;
    }

    public override async Task<bool> UpdateAsync(Todo todo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todo);
        ArgumentOutOfRangeException.ThrowIfNegative((int)todo.Priority, nameof(todo.Priority));

        var existingTodo = await DbContext.Todos.FindAsync(todo.Id, cancellationToken);

        if (existingTodo == null)
            throw new InvalidOperationException(TodoMessage.TodoNotFound);

        existingTodo.Name = todo.Name;
        existingTodo.Done = todo.Done;
        existingTodo.Priority = todo.Priority;
        existingTodo.LastModified = DateTime.UtcNow;

        DbContext.Todos.Update(existingTodo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
