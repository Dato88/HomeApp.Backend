using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class updatedOldTablesToUseSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "budget");

            migrationBuilder.EnsureSchema(
                name: "people");

            migrationBuilder.EnsureSchema(
                name: "todo");

            migrationBuilder.RenameTable(
                name: "Todos",
                schema: "public",
                newName: "todos",
                newSchema: "todo");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "public",
                newName: "people",
                newSchema: "people");

            migrationBuilder.RenameTable(
                name: "Budgets",
                schema: "public",
                newName: "budgets",
                newSchema: "budget");

            migrationBuilder.RenameTable(
                name: "TodoPeople",
                schema: "public",
                newName: "todo_people",
                newSchema: "todo");

            migrationBuilder.RenameTable(
                name: "TodoGroupTodos",
                schema: "public",
                newName: "todo_group_todos",
                newSchema: "todo");

            migrationBuilder.RenameTable(
                name: "TodoGroups",
                schema: "public",
                newName: "todo_groups",
                newSchema: "todo");

            migrationBuilder.RenameTable(
                name: "BudgetRows",
                schema: "public",
                newName: "budget_rows",
                newSchema: "budget");

            migrationBuilder.RenameTable(
                name: "BudgetGroups",
                schema: "public",
                newName: "budget_groups",
                newSchema: "budget");

            migrationBuilder.RenameTable(
                name: "BudgetCells",
                schema: "public",
                newName: "budget_cells",
                newSchema: "budget");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "todos",
                schema: "todo",
                newName: "Todos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "people",
                schema: "people",
                newName: "People",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "budgets",
                schema: "budget",
                newName: "Budgets",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "todo_people",
                schema: "todo",
                newName: "TodoPeople",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "todo_groups",
                schema: "todo",
                newName: "TodoGroups",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "todo_group_todos",
                schema: "todo",
                newName: "TodoGroupTodos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "budget_rows",
                schema: "budget",
                newName: "BudgetRows",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "budget_groups",
                schema: "budget",
                newName: "BudgetGroups",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "budget_cells",
                schema: "budget",
                newName: "BudgetCells",
                newSchema: "public");
        }
    }
}
