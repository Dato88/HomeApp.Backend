using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces.People;
using HomeApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds.People;

public class PersonQueries(HomeAppContext dbContext) : BaseQueries<Person>(dbContext), IPersonQueries
{
    public override async Task<Person> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.People.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var user = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user is null)
            throw new InvalidOperationException(PersonMessage.PersonNotFound);

        return user;
    }

    public async Task<Person> FindByEmailAsync(string email, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.People.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var user = await query.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
            throw new InvalidOperationException(PersonMessage.PersonNotFound);

        return user;
    }

    public override Task GetAllAsync(int id, CancellationToken cancellationToken, bool asNoTracking = true,
        params string[] includes) => throw new NotImplementedException();

    protected override IQueryable<Person> ApplyIncludes(IQueryable<Person> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<Person, object>>>
        {
            { nameof(Person.BudgetCells), x => x.BudgetCells },
            { nameof(Person.BudgetGroups), x => x.BudgetGroups },
            { nameof(Person.BudgetRows), x => x.BudgetRows },
            { nameof(Person.TodoPeople), x => x.TodoPeople }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query.Include(includeMappings[include]);

        return query;
    }
}
