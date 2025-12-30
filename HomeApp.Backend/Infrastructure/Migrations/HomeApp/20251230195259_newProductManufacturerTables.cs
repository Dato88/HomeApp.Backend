using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class newProductManufacturerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_articles_name",
                schema: "article",
                table: "articles");

            migrationBuilder.AddColumn<int>(
                name: "manufacturer_id",
                schema: "article",
                table: "articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "product_id",
                schema: "article",
                table: "articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "manufacturers",
                schema: "article",
                columns: table => new
                {
                    manufacturer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    website_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manufacturers", x => x.manufacturer_id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "article",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    article_category_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.product_id);
                    table.ForeignKey(
                        name: "fk_products_article_categories_article_category_id",
                        column: x => x.article_category_id,
                        principalSchema: "article",
                        principalTable: "article_categories",
                        principalColumn: "article_category_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ix_articles_manufacturer_id",
                schema: "article",
                table: "articles",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "ix_articles_name",
                schema: "article",
                table: "articles",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_articles_product_id",
                schema: "article",
                table: "articles",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_articles_product_id_manufacturer_id",
                schema: "article",
                table: "articles",
                columns: new[] { "product_id", "manufacturer_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_manufacturers_name",
                schema: "article",
                table: "manufacturers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_article_category_id",
                schema: "article",
                table: "products",
                column: "article_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_name",
                schema: "article",
                table: "products",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_articles_manufacturers_manufacturer_id",
                schema: "article",
                table: "articles",
                column: "manufacturer_id",
                principalSchema: "article",
                principalTable: "manufacturers",
                principalColumn: "manufacturer_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_articles_products_product_id",
                schema: "article",
                table: "articles",
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
                name: "fk_articles_manufacturers_manufacturer_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "fk_articles_products_product_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropTable(
                name: "manufacturers",
                schema: "article");

            migrationBuilder.DropTable(
                name: "products",
                schema: "article");

            migrationBuilder.DropIndex(
                name: "ix_articles_manufacturer_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropIndex(
                name: "ix_articles_name",
                schema: "article",
                table: "articles");

            migrationBuilder.DropIndex(
                name: "ix_articles_product_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropIndex(
                name: "ix_articles_product_id_manufacturer_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "manufacturer_id",
                schema: "article",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "product_id",
                schema: "article",
                table: "articles");

            migrationBuilder.CreateIndex(
                name: "ix_articles_name",
                schema: "article",
                table: "articles",
                column: "name",
                unique: true);
        }
    }
}
