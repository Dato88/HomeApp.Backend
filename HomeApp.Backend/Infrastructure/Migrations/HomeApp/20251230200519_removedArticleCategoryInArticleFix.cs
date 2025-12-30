using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class removedArticleCategoryInArticleFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_articles_article_categories_article_category_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropIndex(
                name: "ix_articles_article_category_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "article_category_id",
                schema: "article",
                table: "articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "article_category_id",
                schema: "article",
                table: "articles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_articles_article_category_id",
                schema: "article",
                table: "articles",
                column: "article_category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_articles_article_categories_article_category_id",
                schema: "article",
                table: "articles",
                column: "article_category_id",
                principalSchema: "article",
                principalTable: "article_categories",
                principalColumn: "article_category_id");
        }
    }
}
