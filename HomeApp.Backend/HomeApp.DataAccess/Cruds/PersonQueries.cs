using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.PersonDtos;
using HomeApp.DataAccess.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds;

public class PersonQueries(HomeAppContext dbContext, IPersonValidation personValidation)
    : BaseQueriesOld<Person>(dbContext), IPersonCrud
{
    private readonly IPersonValidation _personValidation = personValidation;

    public override async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var user = await DbContext.People.FindAsync(id, cancellationToken);

        if (user == null)
            throw new InvalidOperationException(PersonMessage.PersonNotFound);

        DbContext.People.Remove(user);
        await DbContext.SaveChangesAsync(cancellationToken);
    }


    public override async Task<PersonDto> FindByIdAsync(int id, CancellationToken cancellationToken,
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

    public async Task<PersonDto> FindByEmailAsync(string email, CancellationToken cancellationToken,
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

    public override async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.People.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return (await query.ToListAsync(cancellationToken)).Select(s => (PersonDto)s);
    }

    public override async Task CreateAsync(Person person, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(person);

        _personValidation.ValidateRequiredProperties(person);
        _personValidation.ValidateMaxLength(person);
        await _personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);
        _personValidation.ValidateEmailFormat(person.Email);

        DbContext.People.Add(person);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(Person person, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(person);

        _personValidation.ValidateRequiredProperties(person);
        _personValidation.ValidateMaxLength(person);
        _personValidation.ValidateEmailFormat(person.Email);

        var existingUser = await DbContext.People.FindAsync(person.Id, cancellationToken);

        if (existingUser == null)
            throw new InvalidOperationException(PersonMessage.PersonNotFound);

        if (person.Username != existingUser.Username)
            await _personValidation.ValidatePersonnameDoesNotExistAsync(person.Username, cancellationToken);

        existingUser.Username = person.Username;
        existingUser.FirstName = person.FirstName;
        existingUser.LastName = person.LastName;
        existingUser.Email = person.Email;

        DbContext.People.Update(existingUser);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

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
                query = query.Include(includeMappings[include]);

        return query;
    }
}
