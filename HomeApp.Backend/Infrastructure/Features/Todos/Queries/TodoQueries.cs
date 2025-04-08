using System.Linq.Expressions;
using Application.Features.Todos.Queries;
using Domain.Entities.Todos;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Todos.Queries;

public class TodoQueries(HomeAppContext dbContext) : BaseQueries<Todo>(dbContext), ITodoQueries
{
    public override async Task<Result<Todo>> FindByIdAsync(
        int id,
        CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        if (id <= 0)
            return Result.Failure<Todo>(TodoErrors.NotFoundById(id));

        var query = DbContext.Todos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var todo = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todo is null)
            return Result.Failure<Todo>(TodoErrors.NotFoundById(id));

        return Result.Success(todo);
    }

    public override async Task<Result<IEnumerable<Todo>>> GetAllAsync(
        int personId,
        CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.Todos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);
        else
            query = query.Include(i => i.TodoGroupTodo)
                .Include(i => i.TodoPeople);

        var todoPeople = await query
            .Where(x => x.TodoPeople.Any(p => p.PersonId == personId))
            .ToListAsync(cancellationToken);

        if (!todoPeople.Any())
            return Result.Failure<IEnumerable<Todo>>(TodoErrors.NotFoundAll);

        return Result.Success<IEnumerable<Todo>>(todoPeople);
    }

    protected override IQueryable<Todo> ApplyIncludes(IQueryable<Todo> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<Todo, object>>>
        {
            { nameof(Todo.TodoGroupTodo), x => x.TodoGroupTodo }, { nameof(Todo.TodoPeople), x => x.TodoPeople }
        };

        foreach (var include in includes)
            if (includeMappings.TryGetValue(include, out var expression))
                query = query.Include(expression);

        return query;
    }
}
