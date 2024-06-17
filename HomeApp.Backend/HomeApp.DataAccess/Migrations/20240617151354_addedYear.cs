using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "BudgetRows",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "BudgetCells",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "BudgetRows");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BudgetCells");
        }
    }
}
