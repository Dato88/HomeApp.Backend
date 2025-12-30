using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class removedArticleCategoryInArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_articles_article_categories_article_category_id",
                schema: "article",
                table: "articles");

            migrationBuilder.AddForeignKey(
                name: "fk_articles_article_categories_article_category_id",
                schema: "article",
                table: "articles",
                column: "article_category_id",
                principalSchema: "article",
                principalTable: "article_categories",
                principalColumn: "article_category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_articles_article_categories_article_category_id",
                schema: "article",
                table: "articles");

            migrationBuilder.AddForeignKey(
                name: "fk_articles_article_categories_article_category_id",
                schema: "article",
                table: "articles",
                column: "article_category_id",
                principalSchema: "article",
                principalTable: "article_categories",
                principalColumn: "article_category_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
