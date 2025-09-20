using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoPersonConfiguration : IEntityTypeConfiguration<TodoPerson>
{
    public void Configure(EntityTypeBuilder<TodoPerson> builder)
    {
        builder.ToTable("TodoPeople");

        builder.HasKey(t => t.TodoPersonId);

        builder.Property(x => x.PersonId)
            .IsRequired();
        builder.Property(x => x.TodoId)
            .IsRequired();

        // Auditing
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(c => c.CreatedById)
            .IsRequired();
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.UpdatedById);

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
