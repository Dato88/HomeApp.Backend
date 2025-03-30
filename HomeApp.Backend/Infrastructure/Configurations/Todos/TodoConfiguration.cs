using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("Todos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Done)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired();

        builder.Property(t => t.LastModified)
            .HasDefaultValueSql("NOW()");

        // Indices
        builder.HasIndex(t => t.Id)
            .IsUnique();
        builder.HasIndex(t => t.Name);
        builder.HasIndex(t => t.Done);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.LastModified);

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
