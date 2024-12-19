using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class TodoGroupCrud(HomeAppContext context) : BaseCrud<TodoGroup>(context), ITodoGroupCrud
{
    public override async Task<TodoGroup> CreateAsync(TodoGroup todoGroup, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroup);

        _context.TodoGroups.Add(todoGroup);
        await _context.SaveChangesAsync(cancellationToken);

        return todoGroup;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroup.Id));

        var todoGroup = await _context.TodoGroups.FindAsync(id, cancellationToken);

        if (todoGroup == null)
            throw new InvalidOperationException("TodoGroup not found");

        _context.TodoGroups.Remove(todoGroup);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<TodoGroup> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoGroup.Id));

        var query = _context.TodoGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes.Length > 0)
            query = ApplyIncludes(query, includes);

        var todoGroup = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todoGroup == null)
            throw new InvalidOperationException("TodoGroup not found");

        return todoGroup;
    }

    public override async Task<IEnumerable<TodoGroup>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        var query = _context.TodoGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes.Length > 0)
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(TodoGroup todoGroup, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoGroup);

        var existingTodoGroup = await _context.TodoGroups.FindAsync(todoGroup.Id, cancellationToken);

        if (existingTodoGroup == null)
            throw new InvalidOperationException("TodoGroup not found");

        existingTodoGroup.Name = todoGroup.Name;

        _context.TodoGroups.Update(existingTodoGroup);
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<TodoGroup> ApplyIncludes(IQueryable<TodoGroup> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<TodoGroup, object>>>
        {
            { nameof(TodoGroup.Todos), x => x.Todos }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
