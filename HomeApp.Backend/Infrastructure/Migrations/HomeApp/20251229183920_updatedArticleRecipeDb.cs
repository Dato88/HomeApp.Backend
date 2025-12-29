using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class updatedArticleRecipeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_recipe_categories_categories_category_id",
                schema: "recipes_core",
                table: "recipe_categories");

            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ingredients_ingredients_ingredient_id",
                schema: "recipes_core",
                table: "recipe_ingredients");

            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ingredients_units_unit_id",
                schema: "recipes_core",
                table: "recipe_ingredients");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "favorites",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "ingredient_allergens",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "ingredient_nutrition",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "ingredient_prices",
                schema: "recipes_pricing");

            migrationBuilder.DropTable(
                name: "ingredients",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "ingredient_categories",
                schema: "recipes_ref");

            migrationBuilder.DropIndex(
                name: "ix_units_name",
                schema: "recipes_ref",
                table: "units");

            migrationBuilder.DropCheckConstraint(
                name: "ck_recipe_ratings_rating",
                schema: "recipes_core",
                table: "recipe_ratings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_recipe_categories",
                schema: "recipes_core",
                table: "recipe_categories");

            migrationBuilder.DropIndex(
                name: "ix_recipe_categories_category_id",
                schema: "recipes_core",
                table: "recipe_categories");

            migrationBuilder.EnsureSchema(
                name: "article");

            migrationBuilder.EnsureSchema(
                name: "recipe");

            migrationBuilder.RenameTable(
                name: "units",
                schema: "recipes_ref",
                newName: "units",
                newSchema: "article");

            migrationBuilder.RenameTable(
                name: "tags",
                schema: "recipes_ref",
                newName: "tags",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "stores",
                schema: "recipes_pricing",
                newName: "stores",
                newSchema: "article");

            migrationBuilder.RenameTable(
                name: "recipes",
                schema: "recipes_core",
                newName: "recipes",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_tags",
                schema: "recipes_core",
                newName: "recipe_tags",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_steps",
                schema: "recipes_core",
                newName: "recipe_steps",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_search_index",
                schema: "recipes_search",
                newName: "recipe_search_index",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_ratings",
                schema: "recipes_core",
                newName: "recipe_ratings",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_ingredients",
                schema: "recipes_core",
                newName: "recipe_ingredients",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_images",
                schema: "recipes_core",
                newName: "recipe_images",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_comments",
                schema: "recipes_core",
                newName: "recipe_comments",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "recipe_categories",
                schema: "recipes_core",
                newName: "recipe_categories",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "popular_search_queries",
                schema: "recipes_search",
                newName: "popular_search_queries",
                newSchema: "recipe");

            migrationBuilder.RenameTable(
                name: "allergens",
                schema: "recipes_ref",
                newName: "allergens",
                newSchema: "article");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "recipe",
                table: "recipes",
                newName: "person_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "recipe",
                table: "recipe_ratings",
                newName: "person_id");

            migrationBuilder.RenameColumn(
                name: "ingredient_id",
                schema: "recipe",
                table: "recipe_ingredients",
                newName: "article_id");

            migrationBuilder.RenameIndex(
                name: "ix_recipe_ingredients_ingredient_id",
                schema: "recipe",
                table: "recipe_ingredients",
                newName: "ix_recipe_ingredients_article_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "recipe",
                table: "recipe_comments",
                newName: "person_id");

            migrationBuilder.RenameColumn(
                name: "comment_id",
                schema: "recipe",
                table: "recipe_comments",
                newName: "recipe_comment_id");

            migrationBuilder.RenameColumn(
                name: "category_id",
                schema: "recipe",
                table: "recipe_categories",
                newName: "recipe_category_id");

            migrationBuilder.AlterColumn<string>(
                name: "abbreviation",
                schema: "article",
                table: "units",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "recipe",
                table: "tags",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "website_url",
                schema: "article",
                table: "stores",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "article",
                table: "stores",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "servings",
                schema: "recipe",
                table: "recipes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_public",
                schema: "recipe",
                table: "recipes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "person_id1",
                schema: "recipe",
                table: "recipes",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "recipe",
                table: "recipe_search_index",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_indexed_at",
                schema: "recipe",
                table: "recipe_search_index",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "rated_at",
                schema: "recipe",
                table: "recipe_ratings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<int>(
                name: "person_id1",
                schema: "recipe",
                table: "recipe_ratings",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sort_order",
                schema: "recipe",
                table: "recipe_ingredients",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "recipe",
                table: "recipe_ingredients",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "person_id1",
                schema: "recipe",
                table: "recipe_comments",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by_id",
                schema: "recipe",
                table: "recipe_categories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "recipe",
                table: "recipe_categories",
                type: "timestamp(3) with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "created_by_id",
                schema: "recipe",
                table: "recipe_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "recipe",
                table: "recipe_categories",
                type: "timestamp(3) with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldDefaultValueSql: "now()")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                schema: "recipe",
                table: "recipe_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "recipe_category_id",
                schema: "recipe",
                table: "recipe_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "recipe",
                table: "recipe_categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "search_count",
                schema: "recipe",
                table: "popular_search_queries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "query_text",
                schema: "recipe",
                table: "popular_search_queries",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_searched_at",
                schema: "recipe",
                table: "popular_search_queries",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                schema: "article",
                table: "allergens",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_recipe_categories",
                schema: "recipe",
                table: "recipe_categories",
                column: "recipe_category_id");

            migrationBuilder.CreateTable(
                name: "article_categories",
                schema: "article",
                columns: table => new
                {
                    article_category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    parent_category_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article_categories", x => x.article_category_id);
                    table.ForeignKey(
                        name: "fk_article_categories_article_categories_parent_category_id",
                        column: x => x.parent_category_id,
                        principalSchema: "article",
                        principalTable: "article_categories",
                        principalColumn: "article_category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recipe_favorites",
                schema: "recipe",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    person_id1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_favorites", x => new { x.person_id, x.recipe_id });
                    table.ForeignKey(
                        name: "fk_recipe_favorites_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_favorites_people_person_id1",
                        column: x => x.person_id1,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id");
                    table.ForeignKey(
                        name: "fk_recipe_favorites_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipe",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articles",
                schema: "article",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    article_category_id = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_articles", x => x.article_id);
                    table.ForeignKey(
                        name: "fk_articles_article_categories_article_category_id",
                        column: x => x.article_category_id,
                        principalSchema: "article",
                        principalTable: "article_categories",
                        principalColumn: "article_category_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "article_allergens",
                schema: "article",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "integer", nullable: false),
                    allergen_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article_allergens", x => new { x.article_id, x.allergen_id });
                    table.ForeignKey(
                        name: "fk_article_allergens_allergens_allergen_id",
                        column: x => x.allergen_id,
                        principalSchema: "article",
                        principalTable: "allergens",
                        principalColumn: "allergen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_article_allergens_articles_article_id",
                        column: x => x.article_id,
                        principalSchema: "article",
                        principalTable: "articles",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article_nutritions",
                schema: "article",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    calories_kcal_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    protein_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    carbs_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    sugar_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fat_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    saturated_fat_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fiber_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    salt_g_per_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    last_source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article_nutritions", x => x.article_id);
                    table.ForeignKey(
                        name: "fk_article_nutritions_articles_article_id",
                        column: x => x.article_id,
                        principalSchema: "article",
                        principalTable: "articles",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article_prices",
                schema: "article",
                columns: table => new
                {
                    article_price_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    article_id = table.Column<int>(type: "integer", nullable: false),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article_prices", x => x.article_price_id);
                    table.ForeignKey(
                        name: "fk_article_prices_articles_article_id",
                        column: x => x.article_id,
                        principalSchema: "article",
                        principalTable: "articles",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_article_prices_stores_store_id",
                        column: x => x.store_id,
                        principalSchema: "article",
                        principalTable: "stores",
                        principalColumn: "store_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_article_prices_units_unit_id",
                        column: x => x.unit_id,
                        principalSchema: "article",
                        principalTable: "units",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_recipes_is_public",
                schema: "recipe",
                table: "recipes",
                column: "is_public");

            migrationBuilder.CreateIndex(
                name: "ix_recipes_person_id1",
                schema: "recipe",
                table: "recipes",
                column: "person_id1");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_ratings_person_id1",
                schema: "recipe",
                table: "recipe_ratings",
                column: "person_id1");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_ingredients_recipe_id_sort_order",
                schema: "recipe",
                table: "recipe_ingredients",
                columns: new[] { "recipe_id", "sort_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recipe_comments_person_id1",
                schema: "recipe",
                table: "recipe_comments",
                column: "person_id1");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_categories_recipe_id_name",
                schema: "recipe",
                table: "recipe_categories",
                columns: new[] { "recipe_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_popular_search_queries_query_text",
                schema: "recipe",
                table: "popular_search_queries",
                column: "query_text",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_allergens_code",
                schema: "article",
                table: "allergens",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_article_allergens_allergen_id",
                schema: "article",
                table: "article_allergens",
                column: "allergen_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_allergens_article_id",
                schema: "article",
                table: "article_allergens",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_categories_name",
                schema: "article",
                table: "article_categories",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_article_categories_parent_category_id",
                schema: "article",
                table: "article_categories",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_prices_article_id",
                schema: "article",
                table: "article_prices",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_prices_article_id_store_id_unit_id_valid_from",
                schema: "article",
                table: "article_prices",
                columns: new[] { "article_id", "store_id", "unit_id", "valid_from" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_article_prices_store_id",
                schema: "article",
                table: "article_prices",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_prices_unit_id",
                schema: "article",
                table: "article_prices",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_articles_article_category_id",
                schema: "article",
                table: "articles",
                column: "article_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_articles_name",
                schema: "article",
                table: "articles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recipe_favorites_person_id1",
                schema: "recipe",
                table: "recipe_favorites",
                column: "person_id1");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_favorites_recipe_id",
                schema: "recipe",
                table: "recipe_favorites",
                column: "recipe_id");

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_comments_people_person_id1",
                schema: "recipe",
                table: "recipe_comments",
                column: "person_id1",
                principalSchema: "people",
                principalTable: "people",
                principalColumn: "person_id");

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ingredients_articles_article_id",
                schema: "recipe",
                table: "recipe_ingredients",
                column: "article_id",
                principalSchema: "article",
                principalTable: "articles",
                principalColumn: "article_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ingredients_units_unit_id",
                schema: "recipe",
                table: "recipe_ingredients",
                column: "unit_id",
                principalSchema: "article",
                principalTable: "units",
                principalColumn: "unit_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ratings_people_person_id1",
                schema: "recipe",
                table: "recipe_ratings",
                column: "person_id1",
                principalSchema: "people",
                principalTable: "people",
                principalColumn: "person_id");

            migrationBuilder.AddForeignKey(
                name: "fk_recipes_people_person_id1",
                schema: "recipe",
                table: "recipes",
                column: "person_id1",
                principalSchema: "people",
                principalTable: "people",
                principalColumn: "person_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_recipe_comments_people_person_id1",
                schema: "recipe",
                table: "recipe_comments");

            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ingredients_articles_article_id",
                schema: "recipe",
                table: "recipe_ingredients");

            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ingredients_units_unit_id",
                schema: "recipe",
                table: "recipe_ingredients");

            migrationBuilder.DropForeignKey(
                name: "fk_recipe_ratings_people_person_id1",
                schema: "recipe",
                table: "recipe_ratings");

            migrationBuilder.DropForeignKey(
                name: "fk_recipes_people_person_id1",
                schema: "recipe",
                table: "recipes");

            migrationBuilder.DropTable(
                name: "article_allergens",
                schema: "article");

            migrationBuilder.DropTable(
                name: "article_nutritions",
                schema: "article");

            migrationBuilder.DropTable(
                name: "article_prices",
                schema: "article");

            migrationBuilder.DropTable(
                name: "recipe_favorites",
                schema: "recipe");

            migrationBuilder.DropTable(
                name: "articles",
                schema: "article");

            migrationBuilder.DropTable(
                name: "article_categories",
                schema: "article");

            migrationBuilder.DropIndex(
                name: "ix_recipes_is_public",
                schema: "recipe",
                table: "recipes");

            migrationBuilder.DropIndex(
                name: "ix_recipes_person_id1",
                schema: "recipe",
                table: "recipes");

            migrationBuilder.DropIndex(
                name: "ix_recipe_ratings_person_id1",
                schema: "recipe",
                table: "recipe_ratings");

            migrationBuilder.DropIndex(
                name: "ix_recipe_ingredients_recipe_id_sort_order",
                schema: "recipe",
                table: "recipe_ingredients");

            migrationBuilder.DropIndex(
                name: "ix_recipe_comments_person_id1",
                schema: "recipe",
                table: "recipe_comments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_recipe_categories",
                schema: "recipe",
                table: "recipe_categories");

            migrationBuilder.DropIndex(
                name: "ix_recipe_categories_recipe_id_name",
                schema: "recipe",
                table: "recipe_categories");

            migrationBuilder.DropIndex(
                name: "ix_popular_search_queries_query_text",
                schema: "recipe",
                table: "popular_search_queries");

            migrationBuilder.DropIndex(
                name: "ix_allergens_code",
                schema: "article",
                table: "allergens");

            migrationBuilder.DropColumn(
                name: "person_id1",
                schema: "recipe",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "person_id1",
                schema: "recipe",
                table: "recipe_ratings");

            migrationBuilder.DropColumn(
                name: "person_id1",
                schema: "recipe",
                table: "recipe_comments");

            migrationBuilder.DropColumn(
                name: "name",
                schema: "recipe",
                table: "recipe_categories");

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
                schema: "article",
                newName: "units",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "tags",
                schema: "recipe",
                newName: "tags",
                newSchema: "recipes_ref");

            migrationBuilder.RenameTable(
                name: "stores",
                schema: "article",
                newName: "stores",
                newSchema: "recipes_pricing");

            migrationBuilder.RenameTable(
                name: "recipes",
                schema: "recipe",
                newName: "recipes",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_tags",
                schema: "recipe",
                newName: "recipe_tags",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_steps",
                schema: "recipe",
                newName: "recipe_steps",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_search_index",
                schema: "recipe",
                newName: "recipe_search_index",
                newSchema: "recipes_search");

            migrationBuilder.RenameTable(
                name: "recipe_ratings",
                schema: "recipe",
                newName: "recipe_ratings",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_ingredients",
                schema: "recipe",
                newName: "recipe_ingredients",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_images",
                schema: "recipe",
                newName: "recipe_images",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_comments",
                schema: "recipe",
                newName: "recipe_comments",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "recipe_categories",
                schema: "recipe",
                newName: "recipe_categories",
                newSchema: "recipes_core");

            migrationBuilder.RenameTable(
                name: "popular_search_queries",
                schema: "recipe",
                newName: "popular_search_queries",
                newSchema: "recipes_search");

            migrationBuilder.RenameTable(
                name: "allergens",
                schema: "article",
                newName: "allergens",
                newSchema: "recipes_ref");

            migrationBuilder.RenameColumn(
                name: "person_id",
                schema: "recipes_core",
                table: "recipes",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "person_id",
                schema: "recipes_core",
                table: "recipe_ratings",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "article_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                newName: "ingredient_id");

            migrationBuilder.RenameIndex(
                name: "ix_recipe_ingredients_article_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                newName: "ix_recipe_ingredients_ingredient_id");

            migrationBuilder.RenameColumn(
                name: "person_id",
                schema: "recipes_core",
                table: "recipe_comments",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "recipe_comment_id",
                schema: "recipes_core",
                table: "recipe_comments",
                newName: "comment_id");

            migrationBuilder.RenameColumn(
                name: "recipe_category_id",
                schema: "recipes_core",
                table: "recipe_categories",
                newName: "category_id");

            migrationBuilder.AlterColumn<string>(
                name: "abbreviation",
                schema: "recipes_ref",
                table: "units",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "recipes_ref",
                table: "tags",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "website_url",
                schema: "recipes_pricing",
                table: "stores",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "recipes_pricing",
                table: "stores",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<decimal>(
                name: "servings",
                schema: "recipes_core",
                table: "recipes",
                type: "numeric(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_public",
                schema: "recipes_core",
                table: "recipes",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "recipes_search",
                table: "recipe_search_index",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_indexed_at",
                schema: "recipes_search",
                table: "recipe_search_index",
                type: "timestamp(3) with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "rated_at",
                schema: "recipes_core",
                table: "recipe_ratings",
                type: "timestamp(3) with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "sort_order",
                schema: "recipes_core",
                table: "recipe_ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "recipes_core",
                table: "recipe_ingredients",
                type: "numeric(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by_id",
                schema: "recipes_core",
                table: "recipe_categories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 5)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                schema: "recipes_core",
                table: "recipe_categories",
                type: "timestamp(3) with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                schema: "recipes_core",
                table: "recipe_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "created_by_id",
                schema: "recipes_core",
                table: "recipe_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "recipes_core",
                table: "recipe_categories",
                type: "timestamp(3) with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldDefaultValueSql: "now()")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                schema: "recipes_core",
                table: "recipe_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "search_count",
                schema: "recipes_search",
                table: "popular_search_queries",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "query_text",
                schema: "recipes_search",
                table: "popular_search_queries",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_searched_at",
                schema: "recipes_search",
                table: "popular_search_queries",
                type: "timestamp(3) with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                schema: "recipes_ref",
                table: "allergens",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_recipe_categories",
                schema: "recipes_core",
                table: "recipe_categories",
                columns: new[] { "recipe_id", "category_id" });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "recipes_ref",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "favorites",
                schema: "recipes_core",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favorites", x => new { x.user_id, x.recipe_id });
                    table.ForeignKey(
                        name: "fk_favorites_people_person_id",
                        column: x => x.user_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_favorites_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredient_categories",
                schema: "recipes_ref",
                columns: table => new
                {
                    ingredient_category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    parent_category_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredient_categories", x => x.ingredient_category_id);
                    table.ForeignKey(
                        name: "fk_ingredient_categories_ingredient_categories_parent_category",
                        column: x => x.parent_category_id,
                        principalSchema: "recipes_ref",
                        principalTable: "ingredient_categories",
                        principalColumn: "ingredient_category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                schema: "recipes_ref",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    ingredient_category_id = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredients", x => x.ingredient_id);
                    table.ForeignKey(
                        name: "fk_ingredients_ingredient_categories_ingredient_category_id",
                        column: x => x.ingredient_category_id,
                        principalSchema: "recipes_ref",
                        principalTable: "ingredient_categories",
                        principalColumn: "ingredient_category_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ingredient_allergens",
                schema: "recipes_ref",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "integer", nullable: false),
                    allergen_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredient_allergens", x => new { x.ingredient_id, x.allergen_id });
                    table.ForeignKey(
                        name: "fk_ingredient_allergens_allergens_allergen_id",
                        column: x => x.allergen_id,
                        principalSchema: "recipes_ref",
                        principalTable: "allergens",
                        principalColumn: "allergen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ingredient_allergens_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalSchema: "recipes_ref",
                        principalTable: "ingredients",
                        principalColumn: "ingredient_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredient_nutrition",
                schema: "recipes_ref",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    calories_kcal_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    carbs_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    fat_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    fiber_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    last_source = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    protein_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    salt_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    saturated_fat_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    sugar_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredient_nutrition", x => x.ingredient_id);
                    table.ForeignKey(
                        name: "fk_ingredient_nutrition_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalSchema: "recipes_ref",
                        principalTable: "ingredients",
                        principalColumn: "ingredient_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredient_prices",
                schema: "recipes_pricing",
                columns: table => new
                {
                    ingredient_price_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    ingredient_id = table.Column<int>(type: "integer", nullable: false),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    valid_to = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredient_prices", x => x.ingredient_price_id);
                    table.ForeignKey(
                        name: "fk_ingredient_prices_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalSchema: "recipes_ref",
                        principalTable: "ingredients",
                        principalColumn: "ingredient_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ingredient_prices_stores_store_id",
                        column: x => x.store_id,
                        principalSchema: "recipes_pricing",
                        principalTable: "stores",
                        principalColumn: "store_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ingredient_prices_units_unit_id",
                        column: x => x.unit_id,
                        principalSchema: "recipes_ref",
                        principalTable: "units",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_units_name",
                schema: "recipes_ref",
                table: "units",
                column: "name",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_recipe_ratings_rating",
                schema: "recipes_core",
                table: "recipe_ratings",
                sql: "rating BETWEEN 1 AND 5");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_categories_category_id",
                schema: "recipes_core",
                table: "recipe_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "recipes_ref",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_favorites_recipe_id",
                schema: "recipes_core",
                table: "favorites",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_allergens_allergen_id",
                schema: "recipes_ref",
                table: "ingredient_allergens",
                column: "allergen_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_categories_name",
                schema: "recipes_ref",
                table: "ingredient_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_categories_parent_category_id",
                schema: "recipes_ref",
                table: "ingredient_categories",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_prices_ingredient_store_valid",
                schema: "recipes_pricing",
                table: "ingredient_prices",
                columns: new[] { "ingredient_id", "store_id", "valid_from", "valid_to" });

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_prices_store_id",
                schema: "recipes_pricing",
                table: "ingredient_prices",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_prices_unit_id",
                schema: "recipes_pricing",
                table: "ingredient_prices",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredients_ingredient_category_id",
                schema: "recipes_ref",
                table: "ingredients",
                column: "ingredient_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredients_name",
                schema: "recipes_ref",
                table: "ingredients",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_categories_categories_category_id",
                schema: "recipes_core",
                table: "recipe_categories",
                column: "category_id",
                principalSchema: "recipes_ref",
                principalTable: "categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ingredients_ingredients_ingredient_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                column: "ingredient_id",
                principalSchema: "recipes_ref",
                principalTable: "ingredients",
                principalColumn: "ingredient_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_recipe_ingredients_units_unit_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                column: "unit_id",
                principalSchema: "recipes_ref",
                principalTable: "units",
                principalColumn: "unit_id");
        }
    }
}
