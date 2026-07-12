using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class categoryGroupsReplaceBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_group_id",
                schema: "finance",
                table: "categories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "category_groups",
                schema: "finance",
                columns: table => new
                {
                    category_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    household_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    category_group_type = table.Column<int>(type: "integer", nullable: false),
                    target_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category_groups", x => x.category_group_id);
                    table.ForeignKey(
                        name: "fk_category_groups_households_household_id",
                        column: x => x.household_id,
                        principalSchema: "people",
                        principalTable: "households",
                        principalColumn: "household_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_category_group_id",
                schema: "finance",
                table: "categories",
                column: "category_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_category_groups_household_id_name",
                schema: "finance",
                table: "category_groups",
                columns: new[] { "household_id", "name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_categories_category_groups_category_group_id",
                schema: "finance",
                table: "categories",
                column: "category_group_id",
                principalSchema: "finance",
                principalTable: "category_groups",
                principalColumn: "category_group_id",
                onDelete: ReferentialAction.SetNull);

            // Backfill: category groups replace the manual budget grid. Per household the groups of
            // the LATEST budget year are carried over (groups are year-independent from now on);
            // duplicate titles within that budget are deduped by lowest index, Unknown-typed groups
            // are skipped. Categories are linked through the budget rows they were attached to.
            migrationBuilder.Sql(
                """
                WITH latest_budget AS (
                    SELECT DISTINCT ON (household_id) budget_id, household_id
                    FROM budget.budgets
                    ORDER BY household_id, year DESC
                ),
                source_groups AS (
                    SELECT DISTINCT ON (lb.household_id, bg.title)
                           lb.household_id, bg.title, bg.budget_group_type, bg.target_percent, bg.created_by_id
                    FROM budget.budget_groups bg
                    JOIN latest_budget lb ON lb.budget_id = bg.budget_id
                    WHERE bg.budget_group_type IN (1, 2)
                    ORDER BY lb.household_id, bg.title, bg.index
                )
                INSERT INTO finance.category_groups
                    (household_id, name, category_group_type, target_percent, created_at, created_by_id)
                SELECT household_id, title, budget_group_type, target_percent, now(), created_by_id
                FROM source_groups;

                WITH latest_budget AS (
                    SELECT DISTINCT ON (household_id) budget_id, household_id
                    FROM budget.budgets
                    ORDER BY household_id, year DESC
                )
                UPDATE finance.categories c
                SET category_group_id = cg.category_group_id
                FROM budget.budget_rows br
                JOIN budget.budget_groups bg ON bg.budget_group_id = br.budget_group_id
                JOIN latest_budget lb ON lb.budget_id = bg.budget_id
                JOIN finance.category_groups cg
                     ON cg.household_id = lb.household_id AND cg.name = bg.title
                WHERE br.category_id = c.category_id
                  AND c.household_id = lb.household_id;
                """);

            migrationBuilder.DropTable(
                name: "budget_cells",
                schema: "budget");

            migrationBuilder.DropTable(
                name: "budget_rows",
                schema: "budget");

            migrationBuilder.DropTable(
                name: "budget_groups",
                schema: "budget");

            migrationBuilder.DropTable(
                name: "budgets",
                schema: "budget");

            migrationBuilder.Sql("DROP SCHEMA IF EXISTS budget;");
        }

        /// <inheritdoc />
        /// <remarks>
        /// Lossy: the budget tables are recreated empty — planned SOLL data and the group backfill
        /// cannot be restored (same best-effort stance as the householdSharing migration).
        /// </remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_categories_category_groups_category_group_id",
                schema: "finance",
                table: "categories");

            migrationBuilder.DropTable(
                name: "category_groups",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "ix_categories_category_group_id",
                schema: "finance",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "category_group_id",
                schema: "finance",
                table: "categories");

            migrationBuilder.EnsureSchema(
                name: "budget");

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
                    household_id = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budgets", x => x.budget_id);
                    table.ForeignKey(
                        name: "fk_budgets_households_household_id",
                        column: x => x.household_id,
                        principalSchema: "people",
                        principalTable: "households",
                        principalColumn: "household_id",
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
                    budget_group_type = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    target_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
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
                    category_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.ForeignKey(
                        name: "fk_budget_rows_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "finance",
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
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
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    month = table.Column<int>(type: "integer", nullable: false)
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
                name: "ix_budget_rows_category_id",
                schema: "budget",
                table: "budget_rows",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_budgets_household_id_year",
                schema: "budget",
                table: "budgets",
                columns: new[] { "household_id", "year" },
                unique: true);
        }
    }
}
