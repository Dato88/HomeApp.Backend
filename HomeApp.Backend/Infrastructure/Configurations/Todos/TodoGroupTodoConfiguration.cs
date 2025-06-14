using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoGroupTodoConfiguration : IEntityTypeConfiguration<TodoGroupTodo>
{
    public void Configure(EntityTypeBuilder<TodoGroupTodo> builder)
    {
        builder.ToTable("TodoGroupTodos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.TodoId)
            .IsRequired();

        builder.Property(x => x.TodoGroupId)
            .IsRequired();

        // Indices
        builder.HasIndex(x => x.TodoId);
        builder.HasIndex(x => x.TodoGroupId);

        // Relations
        builder.HasOne(t => t.Todo)
            .WithOne(tgt => tgt.TodoGroupTodo)
            .HasForeignKey<TodoGroupTodo>(t => t.TodoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tg => tg.TodoGroup)
            .WithMany(tgt => tgt.TodoGroupTodos)
            .HasForeignKey(tg => tg.TodoGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
