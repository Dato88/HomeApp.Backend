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
                name: "public");

            migrationBuilder.CreateTable(
                name: "People",
                schema: "public",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                name: "TodoGroups",
                schema: "public",
                columns: table => new
                {
                    todo_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_groups", x => x.todo_group_id);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                schema: "public",
                columns: table => new
                {
                    todo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    done = table.Column<bool>(type: "boolean", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todos", x => x.todo_id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "public",
                columns: table => new
                {
                    budget_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                        principalSchema: "public",
                        principalTable: "People",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoGroupTodos",
                schema: "public",
                columns: table => new
                {
                    todo_group_todo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                        principalSchema: "public",
                        principalTable: "TodoGroups",
                        principalColumn: "todo_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_group_todos_todos_todo_id",
                        column: x => x.todo_id,
                        principalSchema: "public",
                        principalTable: "Todos",
                        principalColumn: "todo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoPeople",
                schema: "public",
                columns: table => new
                {
                    todo_person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                        principalSchema: "public",
                        principalTable: "People",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_people_todos_todo_id",
                        column: x => x.todo_id,
                        principalSchema: "public",
                        principalTable: "Todos",
                        principalColumn: "todo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetGroups",
                schema: "public",
                columns: table => new
                {
                    budget_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    budget_id = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    budget_group_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_groups", x => x.budget_group_id);
                    table.ForeignKey(
                        name: "fk_budget_groups_budgets_budget_id",
                        column: x => x.budget_id,
                        principalSchema: "public",
                        principalTable: "Budgets",
                        principalColumn: "budget_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetRows",
                schema: "public",
                columns: table => new
                {
                    budget_row_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    budget_group_id = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_rows", x => x.budget_row_id);
                    table.ForeignKey(
                        name: "fk_budget_rows_budget_groups_budget_group_id",
                        column: x => x.budget_group_id,
                        principalSchema: "public",
                        principalTable: "BudgetGroups",
                        principalColumn: "budget_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetCells",
                schema: "public",
                columns: table => new
                {
                    budget_cell_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                        principalSchema: "public",
                        principalTable: "BudgetRows",
                        principalColumn: "budget_row_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_row_id",
                schema: "public",
                table: "BudgetCells",
                column: "budget_row_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_row_id_month",
                schema: "public",
                table: "BudgetCells",
                columns: new[] { "budget_row_id", "month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budget_groups_budget_id",
                schema: "public",
                table: "BudgetGroups",
                column: "budget_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_groups_budget_id_index",
                schema: "public",
                table: "BudgetGroups",
                columns: new[] { "budget_id", "index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_budget_group_id",
                schema: "public",
                table: "BudgetRows",
                column: "budget_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_budget_group_id_index",
                schema: "public",
                table: "BudgetRows",
                columns: new[] { "budget_group_id", "index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_budgets_person_id_year",
                schema: "public",
                table: "Budgets",
                columns: new[] { "person_id", "year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_email",
                schema: "public",
                table: "People",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_user_id",
                schema: "public",
                table: "People",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_username",
                schema: "public",
                table: "People",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_groups_name",
                schema: "public",
                table: "TodoGroups",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_todo_groups_todo_group_id",
                schema: "public",
                table: "TodoGroups",
                column: "todo_group_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_group_todos_todo_group_id",
                schema: "public",
                table: "TodoGroupTodos",
                column: "todo_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_group_todos_todo_id",
                schema: "public",
                table: "TodoGroupTodos",
                column: "todo_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_people_person_id",
                schema: "public",
                table: "TodoPeople",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_people_todo_id",
                schema: "public",
                table: "TodoPeople",
                column: "todo_id");

            migrationBuilder.CreateIndex(
                name: "ix_todos_done",
                schema: "public",
                table: "Todos",
                column: "done");

            migrationBuilder.CreateIndex(
                name: "ix_todos_name",
                schema: "public",
                table: "Todos",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_todos_priority",
                schema: "public",
                table: "Todos",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "ix_todos_todo_id",
                schema: "public",
                table: "Todos",
                column: "todo_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todos_updated_at",
                schema: "public",
                table: "Todos",
                column: "updated_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetCells",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TodoGroupTodos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TodoPeople",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BudgetRows",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TodoGroups",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Todos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BudgetGroups",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "public");

            migrationBuilder.DropTable(
                name: "People",
                schema: "public");
        }
    }
}
