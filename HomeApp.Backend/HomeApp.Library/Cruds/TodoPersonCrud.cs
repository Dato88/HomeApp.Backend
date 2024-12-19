using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class TodoPersonCrud(HomeAppContext context)
    : BaseCrud<TodoPerson>(context), ITodoPersonCrud
{
    public override async Task<TodoPerson> CreateAsync(TodoPerson todoPerson, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoPerson);

        _context.TodoPeople.Add(todoPerson);
        await _context.SaveChangesAsync(cancellationToken);

        return todoPerson;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoPerson.Id));

        var todoPerson = await _context.TodoPeople.FindAsync(id, cancellationToken);

        if (todoPerson == null)
            throw new InvalidOperationException("TodoPerson not found.");

        _context.TodoPeople.Remove(todoPerson);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<TodoPerson> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(TodoPerson.Id));

        var query = _context.TodoPeople.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var todoPerson = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todoPerson == null)
            throw new InvalidOperationException("TodoPerson not found.");

        return todoPerson;
    }

    public override async Task<IEnumerable<TodoPerson>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true, params string[] includes)
    {
        var query = _context.TodoPeople.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(TodoPerson todoPerson, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(todoPerson);

        var existingTodoPerson = await _context.TodoPeople.FindAsync(todoPerson.Id, cancellationToken);

        if (existingTodoPerson == null)
            throw new InvalidOperationException("TodoPerson not found.");

        existingTodoPerson.PersonId = todoPerson.PersonId;
        existingTodoPerson.TodoId = todoPerson.TodoId;

        _context.TodoPeople.Update(existingTodoPerson);
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<TodoPerson> ApplyIncludes(IQueryable<TodoPerson> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<TodoPerson, object>>>
        {
            { nameof(TodoPerson.Person), x => x.Person }, { nameof(TodoPerson.Todo), x => x.Todo }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
