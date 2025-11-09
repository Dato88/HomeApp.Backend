using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class updatedRecipeSchemaNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "recipes_ref");

            migrationBuilder.EnsureSchema(
                name: "recipes_core");

            migrationBuilder.EnsureSchema(
                name: "recipes_pricing");

            migrationBuilder.EnsureSchema(
                name: "recipes_search");

            migrationBuilder.RenameTable(
                name: "units",
                schema: "ref",
                newName: "units",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "tags",
                schema: "ref",
                newName: "tags",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "stores",
                schema: "pricing",
                newName: "stores",
                newSchema: "recipes_pricing");

            migrationBuilder.RenameTable(
                name: "recipes",
                schema: "core",
                newName: "recipes",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_tags",
                schema: "core",
                newName: "recipe_tags",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_steps",
                schema: "core",
                newName: "recipe_steps",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_search_index",
                schema: "search",
                newName: "recipe_search_index",
                newSchema: "recipes_search");

            migrationBuilder.RenameTable(
                name: "recipe_ratings",
                schema: "core",
                newName: "recipe_ratings",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_ingredients",
                schema: "core",
                newName: "recipe_ingredients",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_images",
                schema: "core",
                newName: "recipe_images",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_comments",
                schema: "core",
                newName: "recipe_comments",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_categories",
                schema: "core",
                newName: "recipe_categories",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "popular_search_queries",
                schema: "search",
                newName: "popular_search_queries",
                newSchema: "recipes_search");

            migrationBuilder.RenameTable(
                name: "ingredients",
                schema: "ref",
                newName: "ingredients",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "ingredient_prices",
                schema: "pricing",
                newName: "ingredient_prices",
                newSchema: "recipes_pricing");

            migrationBuilder.RenameTable(
                name: "ingredient_nutrition",
                schema: "nutrition",
                newName: "ingredient_nutrition",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "ingredient_categories",
                schema: "ref",
                newName: "ingredient_categories",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "ingredient_allergens",
                schema: "ref",
                newName: "ingredient_allergens",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "favorites",
                schema: "core",
                newName: "favorites",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "categories",
                schema: "ref",
                newName: "categories",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "allergens",
                schema: "ref",
                newName: "allergens",
                newSchema: "recipes_ref");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ref");

            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.EnsureSchema(
                name: "nutrition");

            migrationBuilder.EnsureSchema(
                name: "pricing");

            migrationBuilder.EnsureSchema(
                name: "search");

            migrationBuilder.RenameTable(
                name: "units",
                schema: "recipes_ref",
                newName: "units",
                newSchema: "ref");

            migrationBuilder.RenameTable(
                name: "tags",
                schema: "recipes_ref",
                newName: "tags",
                newSchema: "ref");

            migrationBuilder.RenameTable(
                name: "stores",
                schema: "recipes_pricing",
                newName: "stores",
                newSchema: "pricing");

            migrationBuilder.RenameTable(
                name: "recipes",
                schema: "recipes_core",
                newName: "recipes",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_tags",
                schema: "recipes_core",
                newName: "recipe_tags",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_steps",
                schema: "recipes_core",
                newName: "recipe_steps",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_search_index",
                schema: "recipes_search",
                newName: "recipe_search_index",
                newSchema: "search");

            migrationBuilder.RenameTable(
                name: "recipe_ratings",
                schema: "recipes_core",
                newName: "recipe_ratings",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_ingredients",
                schema: "recipes_core",
                newName: "recipe_ingredients",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_images",
                schema: "recipes_core",
                newName: "recipe_images",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_comments",
                schema: "recipes_core",
                newName: "recipe_comments",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "recipe_categories",
                schema: "recipes_core",
                newName: "recipe_categories",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "popular_search_queries",
                schema: "recipes_search",
                newName: "popular_search_queries",
                newSchema: "search");

            migrationBuilder.RenameTable(
                name: "ingredients",
                schema: "recipes_ref",
                newName: "ingredients",
                newSchema: "ref");

            migrationBuilder.RenameTable(
                name: "ingredient_prices",
                schema: "recipes_pricing",
                newName: "ingredient_prices",
                newSchema: "pricing");

            migrationBuilder.RenameTable(
                name: "ingredient_nutrition",
                schema: "recipes_ref",
                newName: "ingredient_nutrition",
                newSchema: "nutrition");

            migrationBuilder.RenameTable(
                name: "ingredient_categories",
                schema: "recipes_ref",
                newName: "ingredient_categories",
                newSchema: "ref");

            migrationBuilder.RenameTable(
                name: "ingredient_allergens",
                schema: "recipes_ref",
                newName: "ingredient_allergens",
                newSchema: "ref");

            migrationBuilder.RenameTable(
                name: "favorites",
                schema: "recipes_core",
                newName: "favorites",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "categories",
                schema: "recipes_ref",
                newName: "categories",
                newSchema: "ref");

            migrationBuilder.RenameTable(
                name: "allergens",
                schema: "recipes_ref",
                newName: "allergens",
                newSchema: "ref");
        }
    }
}
