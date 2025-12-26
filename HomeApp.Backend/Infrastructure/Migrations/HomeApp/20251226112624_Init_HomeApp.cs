using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class Init_HomeApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "recipes_ref");

            migrationBuilder.EnsureSchema(
                name: "budget");

            migrationBuilder.EnsureSchema(
                name: "recipes_core");

            migrationBuilder.EnsureSchema(
                name: "recipes_pricing");

            migrationBuilder.EnsureSchema(
                name: "people");

            migrationBuilder.EnsureSchema(
                name: "recipes_search");

            migrationBuilder.EnsureSchema(
                name: "todo");

            migrationBuilder.CreateTable(
                name: "allergens",
                schema: "recipes_ref",
                columns: table => new
                {
                    allergen_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_allergens", x => x.allergen_id);
                });

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
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_category_id = table.Column<int>(type: "integer", nullable: true)
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
                name: "people",
                schema: "people",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    first_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    last_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    user_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_people", x => x.person_id);
                });

            migrationBuilder.CreateTable(
                name: "popular_search_queries",
                schema: "recipes_search",
                columns: table => new
                {
                    query_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    query_text = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    search_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    last_searched_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_popular_search_queries", x => x.query_id);
                });

            migrationBuilder.CreateTable(
                name: "stores",
                schema: "recipes_pricing",
                columns: table => new
                {
                    store_id = table.Column<int>(type: "integer", nullable: false)
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
                    table.PrimaryKey("pk_stores", x => x.store_id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "recipes_ref",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "todo_groups",
                schema: "todo",
                columns: table => new
                {
                    todo_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_groups", x => x.todo_group_id);
                });

            migrationBuilder.CreateTable(
                name: "todos",
                schema: "todo",
                columns: table => new
                {
                    todo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    done = table.Column<bool>(type: "boolean", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todos", x => x.todo_id);
                });

            migrationBuilder.CreateTable(
                name: "units",
                schema: "recipes_ref",
                columns: table => new
                {
                    unit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    abbreviation = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_units", x => x.unit_id);
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
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ingredient_category_id = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
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
                name: "budgets",
                schema: "budget",
                columns: table => new
                {
                    budget_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budgets", x => x.budget_id);
                    table.ForeignKey(
                        name: "fk_budgets_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipes",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    prep_time_minutes = table.Column<int>(type: "integer", nullable: true),
                    cook_time_minutes = table.Column<int>(type: "integer", nullable: true),
                    servings = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipes", x => x.recipe_id);
                    table.ForeignKey(
                        name: "fk_recipes_people_person_id",
                        column: x => x.user_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "todo_group_todos",
                schema: "todo",
                columns: table => new
                {
                    todo_group_todo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    todo_id = table.Column<int>(type: "integer", nullable: false),
                    todo_group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_group_todos", x => x.todo_group_todo_id);
                    table.ForeignKey(
                        name: "fk_todo_group_todos_todo_groups_todo_group_id",
                        column: x => x.todo_group_id,
                        principalSchema: "todo",
                        principalTable: "todo_groups",
                        principalColumn: "todo_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_group_todos_todos_todo_id",
                        column: x => x.todo_id,
                        principalSchema: "todo",
                        principalTable: "todos",
                        principalColumn: "todo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "todo_people",
                schema: "todo",
                columns: table => new
                {
                    todo_person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    todo_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_people", x => x.todo_person_id);
                    table.ForeignKey(
                        name: "fk_todo_people_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_people_todos_todo_id",
                        column: x => x.todo_id,
                        principalSchema: "todo",
                        principalTable: "todos",
                        principalColumn: "todo_id",
                        onDelete: ReferentialAction.Cascade);
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
                    protein_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    carbs_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    sugar_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    fat_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    saturated_fat_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    fiber_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    salt_g_per_100g = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    last_source = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
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
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
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

            migrationBuilder.CreateTable(
                name: "budget_groups",
                schema: "budget",
                columns: table => new
                {
                    budget_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    budget_id = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    budget_group_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_groups", x => x.budget_group_id);
                    table.ForeignKey(
                        name: "fk_budget_groups_budgets_budget_id",
                        column: x => x.budget_id,
                        principalSchema: "budget",
                        principalTable: "budgets",
                        principalColumn: "budget_id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "recipe_categories",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_categories", x => new { x.recipe_id, x.category_id });
                    table.ForeignKey(
                        name: "fk_recipe_categories_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "recipes_ref",
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_categories_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_comments",
                schema: "recipes_core",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    comment_text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "fk_recipe_comments_people_person_id",
                        column: x => x.user_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_comments_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_images",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_image_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_images", x => x.recipe_image_id);
                    table.ForeignKey(
                        name: "fk_recipe_images_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_ingredients",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_ingredient_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_ingredients", x => x.recipe_ingredient_id);
                    table.ForeignKey(
                        name: "fk_recipe_ingredients_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalSchema: "recipes_ref",
                        principalTable: "ingredients",
                        principalColumn: "ingredient_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_ingredients_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_ingredients_units_unit_id",
                        column: x => x.unit_id,
                        principalSchema: "recipes_ref",
                        principalTable: "units",
                        principalColumn: "unit_id");
                });

            migrationBuilder.CreateTable(
                name: "recipe_ratings",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    rating = table.Column<short>(type: "smallint", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_ratings", x => new { x.recipe_id, x.user_id });
                    table.CheckConstraint("ck_recipe_ratings_rating", "rating BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "fk_recipe_ratings_people_person_id",
                        column: x => x.user_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_ratings_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_search_index",
                schema: "recipes_search",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    last_indexed_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_search_index", x => x.recipe_id);
                    table.ForeignKey(
                        name: "fk_recipe_search_index_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_steps",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_step_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    step_number = table.Column<int>(type: "integer", nullable: false),
                    instruction = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_steps", x => x.recipe_step_id);
                    table.ForeignKey(
                        name: "fk_recipe_steps_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_tags",
                schema: "recipes_core",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_tags", x => new { x.recipe_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_recipe_tags_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalSchema: "recipes_core",
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "recipes_ref",
                        principalTable: "tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "budget_rows",
                schema: "budget",
                columns: table => new
                {
                    budget_row_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    budget_group_id = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_rows", x => x.budget_row_id);
                    table.ForeignKey(
                        name: "fk_budget_rows_budget_groups_budget_group_id",
                        column: x => x.budget_group_id,
                        principalSchema: "budget",
                        principalTable: "budget_groups",
                        principalColumn: "budget_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "budget_cells",
                schema: "budget",
                columns: table => new
                {
                    budget_cell_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    budget_row_id = table.Column<int>(type: "integer", nullable: false),
                    month = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_cells", x => x.budget_cell_id);
                    table.CheckConstraint("ck_budgetcell_month", "month BETWEEN 1 AND 12");
                    table.ForeignKey(
                        name: "fk_budget_cells_budget_rows_budget_row_id",
                        column: x => x.budget_row_id,
                        principalSchema: "budget",
                        principalTable: "budget_rows",
                        principalColumn: "budget_row_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_allergens_name",
                schema: "recipes_ref",
                table: "allergens",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_row_id",
                schema: "budget",
                table: "budget_cells",
                column: "budget_row_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_row_id_month",
                schema: "budget",
                table: "budget_cells",
                columns: new[] { "budget_row_id", "month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budget_groups_budget_id",
                schema: "budget",
                table: "budget_groups",
                column: "budget_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_groups_budget_id_index",
                schema: "budget",
                table: "budget_groups",
                columns: new[] { "budget_id", "index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_budget_group_id",
                schema: "budget",
                table: "budget_rows",
                column: "budget_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_budget_group_id_index",
                schema: "budget",
                table: "budget_rows",
                columns: new[] { "budget_group_id", "index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budgets_person_id_year",
                schema: "budget",
                table: "budgets",
                columns: new[] { "person_id", "year" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "ix_people_email",
                schema: "people",
                table: "people",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_user_id",
                schema: "people",
                table: "people",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_username",
                schema: "people",
                table: "people",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recipe_categories_category_id",
                schema: "recipes_core",
                table: "recipe_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_comments_person_id",
                schema: "recipes_core",
                table: "recipe_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_comments_recipe_id",
                schema: "recipes_core",
                table: "recipe_comments",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_images_recipe_id",
                schema: "recipes_core",
                table: "recipe_images",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_ingredients_ingredient_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_ingredients_recipe_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_ingredients_unit_id",
                schema: "recipes_core",
                table: "recipe_ingredients",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_ratings_person_id",
                schema: "recipes_core",
                table: "recipe_ratings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_steps_recipe_id_step_number",
                schema: "recipes_core",
                table: "recipe_steps",
                columns: new[] { "recipe_id", "step_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recipe_tags_tag_id",
                schema: "recipes_core",
                table: "recipe_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipes_person_id",
                schema: "recipes_core",
                table: "recipes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_stores_name",
                schema: "recipes_pricing",
                table: "stores",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                schema: "recipes_ref",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_group_todos_todo_group_id",
                schema: "todo",
                table: "todo_group_todos",
                column: "todo_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_group_todos_todo_id",
                schema: "todo",
                table: "todo_group_todos",
                column: "todo_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_groups_title",
                schema: "todo",
                table: "todo_groups",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_todo_groups_todo_group_id",
                schema: "todo",
                table: "todo_groups",
                column: "todo_group_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_people_person_id",
                schema: "todo",
                table: "todo_people",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_people_todo_id",
                schema: "todo",
                table: "todo_people",
                column: "todo_id");

            migrationBuilder.CreateIndex(
                name: "ix_todos_done",
                schema: "todo",
                table: "todos",
                column: "done");

            migrationBuilder.CreateIndex(
                name: "ix_todos_priority",
                schema: "todo",
                table: "todos",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "ix_todos_title",
                schema: "todo",
                table: "todos",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_todos_todo_id",
                schema: "todo",
                table: "todos",
                column: "todo_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todos_updated_at",
                schema: "todo",
                table: "todos",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "ix_units_abbreviation",
                schema: "recipes_ref",
                table: "units",
                column: "abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_units_name",
                schema: "recipes_ref",
                table: "units",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "budget_cells",
                schema: "budget");

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
                name: "popular_search_queries",
                schema: "recipes_search");

            migrationBuilder.DropTable(
                name: "recipe_categories",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "recipe_comments",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "recipe_images",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "recipe_ingredients",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "recipe_ratings",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "recipe_search_index",
                schema: "recipes_search");

            migrationBuilder.DropTable(
                name: "recipe_steps",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "recipe_tags",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "todo_group_todos",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "todo_people",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "budget_rows",
                schema: "budget");

            migrationBuilder.DropTable(
                name: "allergens",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "stores",
                schema: "recipes_pricing");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "ingredients",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "units",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "recipes",
                schema: "recipes_core");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "todo_groups",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "todos",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "budget_groups",
                schema: "budget");

            migrationBuilder.DropTable(
                name: "ingredient_categories",
                schema: "recipes_ref");

            migrationBuilder.DropTable(
                name: "budgets",
                schema: "budget");

            migrationBuilder.DropTable(
                name: "people",
                schema: "people");
        }
    }
}
