using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class IdentityForStrongIdsFinalTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "TodoPeople",
                newName: "todo_person_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "TodoGroupTodos",
                newName: "todo_group_todo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "todo_person_id",
                schema: "public",
                table: "TodoPeople",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "todo_group_todo_id",
                schema: "public",
                table: "TodoGroupTodos",
                newName: "id");
        }
    }
}
