using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HomeApp.DataAccess.Models;

public partial class HomeAppContext(DbContextOptions<HomeAppContext> options) : DbContext(options)
{
    public virtual DbSet<Person> Users { get; set; }

    public virtual DbSet<BudgetCell> BudgetCells { get; set; }

    public virtual DbSet<BudgetColumn> BudgetColumns { get; set; }

    public virtual DbSet<BudgetGroup> BudgetGroups { get; set; }

    public virtual DbSet<BudgetRow> BudgetRows { get; set; }

    public virtual DbSet<Todo> Todos { get; set; }

    public virtual DbSet<TodoGroup> TodoGroups { get; set; }

    public virtual DbSet<TodoGroupMapping> TodoGroupMappings { get; set; }

    public virtual DbSet<TodoUserMapping> TodoUserMappings { get; set; }
}
