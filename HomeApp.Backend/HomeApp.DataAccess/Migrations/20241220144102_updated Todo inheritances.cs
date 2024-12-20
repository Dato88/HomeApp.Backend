using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedTodoinheritances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoGroupTodos_TodoId",
                table: "TodoGroupTodos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExecutionDate",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_TodoGroupTodos_TodoId",
                table: "TodoGroupTodos",
                column: "TodoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoGroupTodos_TodoId",
                table: "TodoGroupTodos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExecutionDate",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoGroupTodos_TodoId",
                table: "TodoGroupTodos",
                column: "TodoId");
        }
    }
}
