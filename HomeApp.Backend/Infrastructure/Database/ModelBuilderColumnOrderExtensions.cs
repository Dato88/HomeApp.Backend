using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Database;

public static class ModelBuilderColumnOrderExtensions
{
    public static void ApplyAuditedColumnOrdering(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            var entity = modelBuilder.Entity(clrType);

            // 1) Primary key(s) first (supports composite keys)
            var pk = entityType.FindPrimaryKey();
            var order = 0;
            if (pk is not null)
                foreach (var pkProp in pk.Properties)
                    entity.Property(pkProp.ClrType, pkProp.Name)
                        .HasColumnOrder(order++);

            // 2) Then audit fields for IAudited
            if (typeof(AuditableEntity).IsAssignableFrom(clrType))
            {
                // adjust types/names if your interface differs
                entity.Property(typeof(DateTime), nameof(AuditableEntity.CreatedAt)).HasColumnOrder(order++);
                entity.Property(typeof(int), nameof(AuditableEntity.CreatedById)).HasColumnOrder(order++);
                entity.Property(typeof(DateTime?), nameof(AuditableEntity.UpdatedAt)).HasColumnOrder(order++);
                entity.Property(typeof(int?), nameof(AuditableEntity.UpdatedById)).HasColumnOrder(order++);
            }
        }
    }
}
