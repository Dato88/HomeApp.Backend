using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds;

public class TodoGroupQueries(HomeAppContext dbContext) : BaseQueriesOld<TodoGroup>(dbContext), ITodoGroupCrud
{
    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroup.Id));

        var todoGroup = await DbContext.TodoGroups.FindAsync(id, cancellationToken);

        if (todoGroup == null)
            throw new InvalidOperationException(TodoGroupMessage.TodoGroupNotFound);

        DbContext.TodoGroups.Remove(todoGroup);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<TodoGroup> CreateAsync(TodoGroup todoGroup, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroup);

        DbContext.TodoGroups.Add(todoGroup);
        await DbContext.SaveChangesAsync(cancellationToken);

        return todoGroup;
    }

    public override async Task<TodoGroup> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroup.Id));

        var query = DbContext.TodoGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes.Length > 0)
            query = ApplyIncludes(query, includes);

        var todoGroup = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todoGroup == null)
            throw new InvalidOperationException(TodoGroupMessage.TodoGroupNotFound);

        return todoGroup;
    }

    public override async Task<IEnumerable<TodoGroup>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        var query = DbContext.TodoGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes.Length > 0)
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(TodoGroup todoGroup, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroup);

        var existingTodoGroup = await DbContext.TodoGroups.FindAsync(todoGroup.Id, cancellationToken);

        if (existingTodoGroup == null)
            throw new InvalidOperationException(TodoGroupMessage.TodoGroupNotFound);

        existingTodoGroup.Name = todoGroup.Name;

        DbContext.TodoGroups.Update(existingTodoGroup);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<TodoGroup> ApplyIncludes(IQueryable<TodoGroup> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<TodoGroup, object>>>
        {
            { nameof(TodoGroup.TodoGroupTodos), x => x.TodoGroupTodos }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
