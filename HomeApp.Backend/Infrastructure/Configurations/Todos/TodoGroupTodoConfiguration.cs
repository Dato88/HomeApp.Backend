using Domain.Entities.Todos;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoGroupTodoConfiguration : IEntityTypeConfiguration<TodoGroupTodo>
{
    public void Configure(EntityTypeBuilder<TodoGroupTodo> builder)
    {
        builder.ToTable("todo_group_todos", Schemas.Todo);

        builder.HasKey(x => x.TodoGroupTodoId);

        builder.Property(x => x.TodoGroupTodoId)
            .HasColumnName("todo_group_todo_id");

        builder.Property(x => x.TodoId)
            .HasColumnName("todo_id")
            .IsRequired();

        builder.Property(x => x.TodoGroupId)
            .HasColumnName("todo_group_id")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

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
