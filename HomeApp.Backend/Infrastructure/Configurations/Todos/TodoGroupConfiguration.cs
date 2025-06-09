using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Todos;

internal sealed class TodoGroupConfiguration : IEntityTypeConfiguration<TodoGroup>
{
    public void Configure(EntityTypeBuilder<TodoGroup> builder)
    {
        builder.ToTable("TodoGroups");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        // Indices
        builder.HasIndex(x => x.Id)
            .IsUnique();
        builder.HasIndex(x => x.Name);

        // Relations
        builder.HasMany(tgt => tgt.TodoGroupTodos)
            .WithOne(tg => tg.TodoGroup)
            .HasForeignKey(tg => tg.TodoGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
