using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds.Todos;

public class TodoCommands(HomeAppContext context) : BaseCommands<Todo>(context)
{
    public override async Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todo);
        ArgumentOutOfRangeException.ThrowIfNegative((int)todo.Priority, nameof(todo.Priority));

        if (!todo.TodoPeople.Any(x => x.PersonId > 0))
            throw new InvalidOperationException("Should not be null or empty.");

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync(cancellationToken);

        return todo;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(Todo.Id));

        var todo = await _context.Todos.FindAsync(new object[] { id }, cancellationToken);

        if (todo == null)
            throw new InvalidOperationException(TodoMessage.TodoNotFound);

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task UpdateAsync(Todo todo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todo);
        ArgumentOutOfRangeException.ThrowIfNegative((int)todo.Priority, nameof(todo.Priority));

        var existingTodo = await _context.Todos.FindAsync(todo.Id, cancellationToken);

        if (existingTodo == null)
            throw new InvalidOperationException(TodoMessage.TodoNotFound);

        existingTodo.Name = todo.Name;
        existingTodo.Done = todo.Done;
        existingTodo.Priority = todo.Priority;
        existingTodo.LastModified = DateTimeOffset.UtcNow;

        _context.Todos.Update(existingTodo);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
