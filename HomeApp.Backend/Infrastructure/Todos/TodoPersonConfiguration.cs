using Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Todos;

public class TodoPersonConfiguration : IEntityTypeConfiguration<TodoPerson>
{
    public void Configure(EntityTypeBuilder<TodoPerson> builder)
    {
        builder.ToTable("TodoPeople");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(x => x.PersonId).IsRequired();
        builder.Property(x => x.TodoId).IsRequired();

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
