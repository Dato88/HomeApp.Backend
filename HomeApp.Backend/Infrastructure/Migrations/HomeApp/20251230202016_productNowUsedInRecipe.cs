using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class productNowUsedInRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ingredients_articles_article_id",
                schema: "recipe",
                table: "recipe_ingredients");

            migrationBuilder.RenameColumn(
                name: "article_id",
                schema: "recipe",
                table: "recipe_ingredients",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "ix_recipe_ingredients_article_id",
                schema: "recipe",
                table: "recipe_ingredients",
                newName: "ix_recipe_ingredients_product_id");

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ingredients_products_product_id",
                schema: "recipe",
                table: "recipe_ingredients",
                column: "product_id",
                principalSchema: "article",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ingredients_products_product_id",
                schema: "recipe",
                table: "recipe_ingredients");

            migrationBuilder.RenameColumn(
                name: "product_id",
                schema: "recipe",
                table: "recipe_ingredients",
                newName: "article_id");

            migrationBuilder.RenameIndex(
                name: "ix_recipe_ingredients_product_id",
                schema: "recipe",
                table: "recipe_ingredients",
                newName: "ix_recipe_ingredients_article_id");

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ingredients_articles_article_id",
                schema: "recipe",
                table: "recipe_ingredients",
                column: "article_id",
                principalSchema: "article",
                principalTable: "articles",
                principalColumn: "article_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
