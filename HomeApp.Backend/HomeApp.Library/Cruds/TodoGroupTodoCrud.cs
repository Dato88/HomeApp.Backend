using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class TodoGroupTodoCrud(HomeAppContext context) : BaseCrud<TodoGroupTodo>(context), ITodoGroupTodoCrud
{
    public override async Task<TodoGroupTodo> CreateAsync(TodoGroupTodo todoGroupTodo,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroupTodo);

        _context.TodoGroupTodos.Add(todoGroupTodo);
        await _context.SaveChangesAsync(cancellationToken);

        return todoGroupTodo;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroupTodo.Id));

        var todoGroupTodo = await _context.TodoGroupTodos.FindAsync(id, cancellationToken);

        if (todoGroupTodo == null)
            throw new InvalidOperationException(TodoGroupTodoMessage.TodoGroupTodoNotFound);

        _context.TodoGroupTodos.Remove(todoGroupTodo);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<TodoGroupTodo> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroupTodo.Id));

        var query = _context.TodoGroupTodos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var todoGroupTodo = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todoGroupTodo == null)
            throw new InvalidOperationException(TodoGroupTodoMessage.TodoGroupTodoNotFound);

        return todoGroupTodo;
    }

    public override async Task<IEnumerable<TodoGroupTodo>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        var query = _context.TodoGroupTodos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(TodoGroupTodo todoGroupTodo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroupTodo);

        var existingTodoGroupTodo = await _context.TodoGroupTodos.FindAsync(todoGroupTodo.Id, cancellationToken);

        if (existingTodoGroupTodo == null)
            throw new InvalidOperationException(TodoGroupTodoMessage.TodoGroupTodoNotFound);

        existingTodoGroupTodo.TodoId = todoGroupTodo.TodoId;
        existingTodoGroupTodo.TodoGroupId = todoGroupTodo.TodoGroupId;

        _context.TodoGroupTodos.Update(existingTodoGroupTodo);
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<TodoGroupTodo> ApplyIncludes(IQueryable<TodoGroupTodo> query,
        params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<TodoGroupTodo, object>>>
        {
            { nameof(TodoGroupTodo.Todo), x => x.Todo }, { nameof(TodoGroupTodo.TodoGroup), x => x.TodoGroup }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
