using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds;

public class TodoGroupTodoQueries(HomeAppContext dbContext)
    : BaseQueriesOld<TodoGroupTodo>(dbContext), ITodoGroupTodoCrud
{
    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroupTodo.Id));

        var todoGroupTodo = await DbContext.TodoGroupTodos.FindAsync(id, cancellationToken);

        if (todoGroupTodo == null)
            throw new InvalidOperationException(TodoGroupTodoMessage.TodoGroupTodoNotFound);

        DbContext.TodoGroupTodos.Remove(todoGroupTodo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<TodoGroupTodo> CreateAsync(TodoGroupTodo todoGroupTodo,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroupTodo);

        DbContext.TodoGroupTodos.Add(todoGroupTodo);
        await DbContext.SaveChangesAsync(cancellationToken);

        return todoGroupTodo;
    }

    public override async Task<TodoGroupTodo> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroupTodo.Id));

        var query = DbContext.TodoGroupTodos.AsQueryable();

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
        var query = DbContext.TodoGroupTodos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(TodoGroupTodo todoGroupTodo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroupTodo);

        var existingTodoGroupTodo = await DbContext.TodoGroupTodos.FindAsync(todoGroupTodo.Id, cancellationToken);

        if (existingTodoGroupTodo == null)
            throw new InvalidOperationException(TodoGroupTodoMessage.TodoGroupTodoNotFound);

        existingTodoGroupTodo.TodoId = todoGroupTodo.TodoId;
        existingTodoGroupTodo.TodoGroupId = todoGroupTodo.TodoGroupId;

        DbContext.TodoGroupTodos.Update(existingTodoGroupTodo);
        await DbContext.SaveChangesAsync(cancellationToken);
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
