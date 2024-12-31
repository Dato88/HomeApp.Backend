using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newPropertyLastModifiedInTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionDate",
                table: "Todos");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModified",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Todos");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutionDate",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
