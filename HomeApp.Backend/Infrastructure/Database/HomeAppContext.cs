using Domain.Budgets;
using Domain.People;
using Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class HomeAppContext(DbContextOptions<HomeAppContext> options) : DbContext(options)
{
    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<BudgetCell> BudgetCells { get; set; }
    public virtual DbSet<BudgetColumn> BudgetColumns { get; set; }
    public virtual DbSet<BudgetGroup> BudgetGroups { get; set; }
    public virtual DbSet<BudgetRow> BudgetRows { get; set; }
    public virtual DbSet<Todo> Todos { get; set; }
    public virtual DbSet<TodoGroup> TodoGroups { get; set; }
    public virtual DbSet<TodoGroupTodo> TodoGroupTodos { get; set; }
    public virtual DbSet<TodoPerson> TodoPeople { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeAppContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}
