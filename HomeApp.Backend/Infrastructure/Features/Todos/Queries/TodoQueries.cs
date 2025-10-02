using System.Linq.Expressions;
using Application.Features.Todos.Queries;
using Domain.Entities.Todos;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Todos.Queries;

public sealed class TodoQueries(HomeAppContext dbContext) : ITodoQueries
{
    public async Task<Result<Todo>> FindTodoByIdAsync(int todoId, CancellationToken cancellationToken)
    {
        if (todoId <= 0)
            return Result.Failure<Todo>(TodoErrors.NotFoundById(todoId));

        var todo = await dbContext.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TodoId == todoId, cancellationToken);

        if (todo is null)
            return Result.Failure<Todo>(TodoErrors.NotFoundById(todoId));

        return Result.Success(todo);
    }

    public async Task<Result<IEnumerable<Todo>>> GetAllUserTodosAsync(
        int personId,
        CancellationToken cancellationToken)
    {
        var todos = await dbContext.Todos
            .AsNoTracking()
            .Include(i => i.TodoGroupTodo)
            .Include(i => i.TodoPeople)
            .Where(x => x.TodoPeople.Any(p => p.PersonId == personId))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        if (!todos.Any())
            return Result.Failure<IEnumerable<Todo>>(TodoErrors.NotFoundAll);

        return Result.Success<IEnumerable<Todo>>(todos);
    }
}
