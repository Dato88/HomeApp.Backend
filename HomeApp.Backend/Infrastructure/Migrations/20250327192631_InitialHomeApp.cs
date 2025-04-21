using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialHomeApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "BudgetColumns",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    index = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 905, DateTimeKind.Utc).AddTicks(5870))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_columns", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    first_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    last_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    user_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 906, DateTimeKind.Utc).AddTicks(9500))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_people", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TodoGroups",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(1860))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    done = table.Column<bool>(type: "boolean", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 907, DateTimeKind.Utc).AddTicks(5750)),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 907, DateTimeKind.Utc).AddTicks(5160))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetGroups",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 905, DateTimeKind.Utc).AddTicks(9520))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_budget_groups_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "public",
                        principalTable: "People",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetRows",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 906, DateTimeKind.Utc).AddTicks(4560))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_rows", x => x.id);
                    table.ForeignKey(
                        name: "fk_budget_rows_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "public",
                        principalTable: "People",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoGroupTodos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    todo_id = table.Column<int>(type: "integer", nullable: false),
                    todo_group_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(6110))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_group_todos", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_group_todos_todo_groups_todo_group_id",
                        column: x => x.todo_group_id,
                        principalSchema: "public",
                        principalTable: "TodoGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_group_todos_todos_todo_id",
                        column: x => x.todo_id,
                        principalSchema: "public",
                        principalTable: "Todos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoPeople",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    todo_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(9800))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_people", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_people_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "public",
                        principalTable: "People",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_todo_people_todos_todo_id",
                        column: x => x.todo_id,
                        principalSchema: "public",
                        principalTable: "Todos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetCells",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    budget_row_id = table.Column<int>(type: "integer", nullable: false),
                    budget_column_id = table.Column<int>(type: "integer", nullable: false),
                    budget_group_id = table.Column<int>(type: "integer", nullable: false),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 904, DateTimeKind.Utc).AddTicks(7210))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_budget_cells", x => x.id);
                    table.ForeignKey(
                        name: "fk_budget_cells_budget_columns_budget_column_id",
                        column: x => x.budget_column_id,
                        principalSchema: "public",
                        principalTable: "BudgetColumns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_budget_cells_budget_groups_budget_group_id",
                        column: x => x.budget_group_id,
                        principalSchema: "public",
                        principalTable: "BudgetGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_budget_cells_budget_rows_budget_row_id",
                        column: x => x.budget_row_id,
                        principalSchema: "public",
                        principalTable: "BudgetRows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_budget_cells_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "public",
                        principalTable: "People",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_column_id",
                schema: "public",
                table: "BudgetCells",
                column: "budget_column_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_group_id",
                schema: "public",
                table: "BudgetCells",
                column: "budget_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_budget_row_id",
                schema: "public",
                table: "BudgetCells",
                column: "budget_row_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_person_id",
                schema: "public",
                table: "BudgetCells",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_cells_year",
                schema: "public",
                table: "BudgetCells",
                column: "year");

            migrationBuilder.CreateIndex(
                name: "ix_budget_columns_index",
                schema: "public",
                table: "BudgetColumns",
                column: "index");

            migrationBuilder.CreateIndex(
                name: "ix_budget_columns_name",
                schema: "public",
                table: "BudgetColumns",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_budget_groups_index",
                schema: "public",
                table: "BudgetGroups",
                column: "index");

            migrationBuilder.CreateIndex(
                name: "ix_budget_groups_person_id",
                schema: "public",
                table: "BudgetGroups",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_index",
                schema: "public",
                table: "BudgetRows",
                column: "index");

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_person_id",
                schema: "public",
                table: "BudgetRows",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_budget_rows_year",
                schema: "public",
                table: "BudgetRows",
                column: "year");

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
                name: "ix_todo_groups_id",
                schema: "public",
                table: "TodoGroups",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_groups_name",
                schema: "public",
                table: "TodoGroups",
                column: "name");

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
                name: "ix_todos_id",
                schema: "public",
                table: "Todos",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todos_last_modified",
                schema: "public",
                table: "Todos",
                column: "last_modified");

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
                name: "BudgetColumns",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BudgetGroups",
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
                name: "People",
                schema: "public");
        }
    }
}
