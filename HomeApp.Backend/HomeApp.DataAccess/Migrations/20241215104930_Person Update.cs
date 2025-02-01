using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PersonCommandsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCells_Users_UserId",
                table: "BudgetCells");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetGroups_Users_UserId",
                table: "BudgetGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetRows_Users_UserId",
                table: "BudgetRows");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoUserMapping_Users_UserId",
                table: "TodoUserMapping");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TodoUserMapping_UserId",
                table: "TodoUserMapping");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BudgetRows",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetRows_UserId",
                table: "BudgetRows",
                newName: "IX_BudgetRows_PersonId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BudgetGroups",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetGroups_UserId",
                table: "BudgetGroups",
                newName: "IX_BudgetGroups_PersonId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BudgetCells",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetCells_UserId",
                table: "BudgetCells",
                newName: "IX_BudgetCells_PersonId");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "TodoUserMapping",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoUserMapping_PersonId",
                table: "TodoUserMapping",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCells_People_PersonId",
                table: "BudgetCells",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroups_People_PersonId",
                table: "BudgetGroups",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetRows_People_PersonId",
                table: "BudgetRows",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoUserMapping_People_PersonId",
                table: "TodoUserMapping",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCells_People_PersonId",
                table: "BudgetCells");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetGroups_People_PersonId",
                table: "BudgetGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetRows_People_PersonId",
                table: "BudgetRows");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoUserMapping_People_PersonId",
                table: "TodoUserMapping");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropIndex(
                name: "IX_TodoUserMapping_PersonId",
                table: "TodoUserMapping");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "TodoUserMapping");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "BudgetRows",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetRows_PersonId",
                table: "BudgetRows",
                newName: "IX_BudgetRows_UserId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "BudgetGroups",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetGroups_PersonId",
                table: "BudgetGroups",
                newName: "IX_BudgetGroups_UserId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "BudgetCells",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetCells_PersonId",
                table: "BudgetCells",
                newName: "IX_BudgetCells_UserId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoUserMapping_UserId",
                table: "TodoUserMapping",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCells_Users_UserId",
                table: "BudgetCells",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroups_Users_UserId",
                table: "BudgetGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetRows_Users_UserId",
                table: "BudgetRows",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoUserMapping_Users_UserId",
                table: "TodoUserMapping",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
