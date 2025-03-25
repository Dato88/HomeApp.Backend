using Domain.Entities.Budgets;
using Domain.Entities.People;
using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IHomeAppContext
{
    DbSet<Person> People { get; set; }
    DbSet<BudgetCell> BudgetCells { get; set; }
    DbSet<BudgetColumn> BudgetColumns { get; set; }
    DbSet<BudgetGroup> BudgetGroups { get; set; }
    DbSet<BudgetRow> BudgetRows { get; set; }
    DbSet<Todo> Todos { get; set; }
    DbSet<TodoGroup> TodoGroups { get; set; }
    DbSet<TodoGroupTodo> TodoGroupTodos { get; set; }
    DbSet<TodoPerson> TodoPeople { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
