using Domain.Entities.Budgets;
using Domain.Entities.People;
using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class HomeAppContext(DbContextOptions<HomeAppContext> options) : DbContext(options)
{
    public DbSet<Person> People { get; set; }
    public DbSet<BudgetCell> BudgetCells { get; set; }
    public DbSet<BudgetColumn> BudgetColumns { get; set; }
    public DbSet<BudgetGroup> BudgetGroups { get; set; }
    public DbSet<BudgetRow> BudgetRows { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoGroup> TodoGroups { get; set; }
    public DbSet<TodoGroupTodo> TodoGroupTodos { get; set; }
    public DbSet<TodoPerson> TodoPeople { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeAppContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}
