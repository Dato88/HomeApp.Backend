// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Domain.Entities.Recipes.RecipesPricing;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesPricing;

public class IngredientPriceConfiguration : IEntityTypeConfiguration<IngredientPrice>
{
    public void Configure(EntityTypeBuilder<IngredientPrice> builder)
    {
        builder.ToTable("ingredient_prices", Schemas.RecipesPricing);

        builder.HasKey(x => x.IngredientPriceId);

        builder.Property(x => x.IngredientPriceId)
            .HasColumnName("ingredient_price_id");

        builder.Property(x => x.IngredientId)
            .HasColumnName("ingredient_id")
            .IsRequired();

        builder.Property(x => x.StoreId)
            .HasColumnName("store_id")
            .IsRequired();

        builder.Property(x => x.UnitId)
            .HasColumnName("unit_id")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasColumnName("currency")
            .HasMaxLength(3)
            .HasDefaultValue("EUR")
            .IsRequired();

        builder.Property(x => x.ValidFrom)
            .HasColumnName("valid_from")
            .HasColumnType("timestamp(3) with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(x => x.ValidTo)
            .HasColumnName("valid_to")
            .HasColumnType("timestamp(3) with time zone");

        builder.HasIndex(x => new { x.IngredientId, x.StoreId, x.ValidFrom, x.ValidTo })
            .HasDatabaseName("ix_ingredient_prices_ingredient_store_valid");

        builder.HasOne(x => x.Ingredient)
            .WithMany(x => x.IngredientPrices)
            .HasForeignKey(x => x.IngredientId);

        builder.HasOne(x => x.Store)
            .WithMany(x => x.IngredientPrices)
            .HasForeignKey(x => x.StoreId);

        builder.HasOne(x => x.Unit)
            .WithMany(x => x.IngredientPrices)
            .HasForeignKey(x => x.UnitId);

        builder.ConfigureAuditable();
    }
}
