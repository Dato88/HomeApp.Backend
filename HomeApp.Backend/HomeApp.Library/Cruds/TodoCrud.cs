﻿using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class TodoCrud(HomeAppContext context)
    : BaseCrud<Todo>(context), ITodoCrud
{
    public override async Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todo);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero((int)todo.Priority, nameof(todo.Priority));

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync(cancellationToken);

        return todo;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(Todo.Id));

        var todo = await _context.Todos.FindAsync(new object[] { id }, cancellationToken);

        if (todo == null)
            throw new InvalidOperationException("Todo not found.");

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<Todo> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(Todo.Id));

        var query = _context.Todos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var todo = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todo == null)
            throw new InvalidOperationException("Todo not found.");

        return todo;
    }

    public override Task GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes) => throw new NotImplementedException();

    public override async Task UpdateAsync(Todo todo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todo);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero((int)todo.Priority, nameof(todo.Priority));

        var existingTodo = await _context.Todos.FindAsync(todo.Id, cancellationToken);

        if (existingTodo == null)
            throw new InvalidOperationException("Todo not found.");

        existingTodo.Name = todo.Name;
        existingTodo.Done = todo.Done;
        existingTodo.Priority = todo.Priority;
        existingTodo.ExecutionDate = todo.ExecutionDate;

        _context.Todos.Update(existingTodo);
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<Todo> ApplyIncludes(IQueryable<Todo> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<Todo, object>>>
        {
            { nameof(Todo.TodoGroups), x => x.TodoGroups }, { nameof(Todo.People), x => x.People }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
