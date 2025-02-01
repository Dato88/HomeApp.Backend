﻿using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds.Todos;

public class TodoQueries(HomeAppContext dbContext) : BaseQueries<Todo>(dbContext), ITodoQueries
{
    public override async Task<Todo> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(Todo.Id));

        var query = DbContext.Todos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var todo = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todo == null)
            throw new InvalidOperationException(TodoMessage.TodoNotFound);

        return todo;
    }

    public override async Task<IEnumerable<GetToDoDto>> GetAllAsync(int personId, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.Todos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);
        else
            query.Include(i => i.TodoGroupTodo).Include(i => i.TodoPeople);

        var todoPeople = await query.Where(x => x.TodoPeople.Select(s => s.PersonId).Contains(personId))
            .ToListAsync(cancellationToken);

        return todoPeople.Select(s => (GetToDoDto)s);
    }

    protected override IQueryable<Todo> ApplyIncludes(IQueryable<Todo> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<Todo, object>>>
        {
            { nameof(Todo.TodoGroupTodo), x => x.TodoGroupTodo }, { nameof(Todo.TodoPeople), x => x.TodoPeople }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
