using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class renameNameToTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "Todos",
                newName: "title");

            migrationBuilder.RenameIndex(
                name: "ix_todos_name",
                schema: "public",
                table: "Todos",
                newName: "ix_todos_title");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "TodoGroups",
                newName: "title");

            migrationBuilder.RenameIndex(
                name: "ix_todo_groups_name",
                schema: "public",
                table: "TodoGroups",
                newName: "ix_todo_groups_title");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "BudgetRows",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "BudgetGroups",
                newName: "title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                schema: "public",
                table: "Todos",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "ix_todos_title",
                schema: "public",
                table: "Todos",
                newName: "ix_todos_name");

            migrationBuilder.RenameColumn(
                name: "title",
                schema: "public",
                table: "TodoGroups",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "ix_todo_groups_title",
                schema: "public",
                table: "TodoGroups",
                newName: "ix_todo_groups_name");

            migrationBuilder.RenameColumn(
                name: "title",
                schema: "public",
                table: "BudgetRows",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "title",
                schema: "public",
                table: "BudgetGroups",
                newName: "name");
        }
    }
}
