using Domain.Entities.Todos;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoPersonConfiguration : IEntityTypeConfiguration<TodoPerson>
{
    public void Configure(EntityTypeBuilder<TodoPerson> builder)
    {
        builder.ToTable("todo_people", Schemas.Todo);

        builder.HasKey(x => x.TodoPersonId);

        builder.Property(x => x.TodoPersonId)
            .HasColumnName("todo_person_id");

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(x => x.TodoId)
            .HasColumnName("todo_id")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // Indices
        builder.HasIndex(x => x.PersonId);
        builder.HasIndex(x => x.TodoId);

        // Relations
        builder.HasOne(t => t.Todo)
            .WithMany(tp => tp.TodoPeople)
            .HasForeignKey(t => t.TodoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Person)
            .WithMany(tp => tp.TodoPeople)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
