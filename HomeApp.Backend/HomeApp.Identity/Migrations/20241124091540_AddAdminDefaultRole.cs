using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeApp.Identity.Migrations;

/// <inheritdoc />
public partial class AddAdminDefaultRole : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            schema: "identity",
            table: "AspNetRoles",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[] { "c075b934-a14e-43fc-b0d7-3af122866be2", null, "Admin", "ADMIN" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            schema: "identity",
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "c075b934-a14e-43fc-b0d7-3af122866be2");
    }
}
