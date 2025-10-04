using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoGroupConfiguration : IEntityTypeConfiguration<TodoGroup>
{
    public void Configure(EntityTypeBuilder<TodoGroup> builder)
    {
        builder.ToTable("TodoGroups");

        builder.HasKey(t => t.TodoGroupId);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        // Auditing
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(c => c.CreatedById)
            .IsRequired();
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.UpdatedById);

        // Indices
        builder.HasIndex(x => x.TodoGroupId)
            .IsUnique();
        builder.HasIndex(x => x.Title);

        // Relations
        builder.HasMany(tgt => tgt.TodoGroupTodos)
            .WithOne(tg => tg.TodoGroup)
            .HasForeignKey(tg => tg.TodoGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
