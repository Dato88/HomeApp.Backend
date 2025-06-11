using Domain.Entities.Budgets;
using Domain.Entities.People;
using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        foreach (var prop in entityType.ClrType.GetProperties())
        {
            var propType = prop.PropertyType;
            if (propType.IsValueType && propType.Name.EndsWith("Id"))
            {
                var converterType = typeof(StronglyTypedIdConverter<>).MakeGenericType(propType);
                var converter = (ValueConverter)Activator.CreateInstance(converterType)!;

                var propertyBuilder = modelBuilder.Entity(entityType.ClrType)
                    .Property(prop.Name)
                    .HasConversion(converter);

                propertyBuilder
                    .ValueGeneratedOnAdd()
                    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeAppContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}
