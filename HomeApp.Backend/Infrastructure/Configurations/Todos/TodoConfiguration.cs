using Domain.Entities.Todos;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos", Schemas.Todo);

        builder.HasKey(t => t.TodoId);

        builder.Property(t => t.TodoId)
            .HasColumnName("todo_id");

        builder.Property(t => t.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Done)
            .HasColumnName("done")
            .IsRequired();

        builder.Property(t => t.Priority)
            .HasColumnName("priority")
            .IsRequired();


        // Auditing
        builder.ConfigureAuditable();

        // Indices
        builder.HasIndex(t => t.TodoId)
            .IsUnique();
        builder.HasIndex(t => t.Title);
        builder.HasIndex(t => t.Done);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.UpdatedAt);

        // Relations
        builder.HasOne(tgt => tgt.TodoGroupTodo)
            .WithOne(t => t.Todo)
            .HasForeignKey<TodoGroupTodo>(t => t.TodoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(tp => tp.TodoPeople)
            .WithOne(t => t.Todo)
            .HasForeignKey(t => t.TodoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
