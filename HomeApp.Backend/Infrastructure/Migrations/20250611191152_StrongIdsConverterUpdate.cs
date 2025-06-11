using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class StrongIdsConverterUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "Todos",
                newName: "todo_id");

            migrationBuilder.RenameIndex(
                name: "ix_todos_id",
                schema: "public",
                table: "Todos",
                newName: "ix_todos_todo_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "TodoGroups",
                newName: "todo_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_groups_id",
                schema: "public",
                table: "TodoGroups",
                newName: "ix_todo_groups_todo_group_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "People",
                newName: "person_id");

            migrationBuilder.AlterColumn<int>(
                name: "todo_id",
                schema: "public",
                table: "Todos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "todo_group_id",
                schema: "public",
                table: "TodoGroups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "person_id",
                schema: "public",
                table: "People",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "todo_id",
                schema: "public",
                table: "Todos",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_todos_todo_id",
                schema: "public",
                table: "Todos",
                newName: "ix_todos_id");

            migrationBuilder.RenameColumn(
                name: "todo_group_id",
                schema: "public",
                table: "TodoGroups",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_groups_todo_group_id",
                schema: "public",
                table: "TodoGroups",
                newName: "ix_todo_groups_id");

            migrationBuilder.RenameColumn(
                name: "person_id",
                schema: "public",
                table: "People",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "public",
                table: "Todos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "public",
                table: "TodoGroups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "public",
                table: "People",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
