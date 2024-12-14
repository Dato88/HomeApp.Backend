using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeApp.DataAccess.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "BudgetColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Index = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_BudgetColumns", x => x.Id));

        migrationBuilder.CreateTable(
            name: "TodoGroups",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                TodoGroupName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_TodoGroups", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Todos",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                TodoName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                TodoDone = table.Column<bool>(type: "boolean", nullable: false),
                TodoPriority = table.Column<int>(type: "integer", nullable: false),
                TodoExecutionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Todos", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                FirstName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                LastName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                Password = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        migrationBuilder.CreateTable(
            name: "TodoGroupMapping",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                TodoId = table.Column<int>(type: "integer", nullable: false),
                TodoGroupId = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TodoGroupMapping", x => x.Id);
                table.ForeignKey(
                    name: "FK_TodoGroupMapping_TodoGroups_TodoGroupId",
                    column: x => x.TodoGroupId,
                    principalTable: "TodoGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_TodoGroupMapping_Todos_TodoId",
                    column: x => x.TodoId,
                    principalTable: "Todos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BudgetGroups",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Index = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BudgetGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_BudgetGroups_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BudgetRows",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Index = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BudgetRows", x => x.Id);
                table.ForeignKey(
                    name: "FK_BudgetRows_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TodoUserMapping",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                TodoId = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TodoUserMapping", x => x.Id);
                table.ForeignKey(
                    name: "FK_TodoUserMapping_Todos_TodoId",
                    column: x => x.TodoId,
                    principalTable: "Todos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_TodoUserMapping_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BudgetCells",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                BudgetRowId = table.Column<int>(type: "integer", nullable: false),
                BudgetColumnId = table.Column<int>(type: "integer", nullable: false),
                BudgetGroupId = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BudgetCells", x => x.Id);
                table.ForeignKey(
                    name: "FK_BudgetCells_BudgetColumns_BudgetColumnId",
                    column: x => x.BudgetColumnId,
                    principalTable: "BudgetColumns",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BudgetCells_BudgetGroups_BudgetGroupId",
                    column: x => x.BudgetGroupId,
                    principalTable: "BudgetGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BudgetCells_BudgetRows_BudgetRowId",
                    column: x => x.BudgetRowId,
                    principalTable: "BudgetRows",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BudgetCells_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_BudgetCells_BudgetColumnId",
            table: "BudgetCells",
            column: "BudgetColumnId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetCells_BudgetGroupId",
            table: "BudgetCells",
            column: "BudgetGroupId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetCells_BudgetRowId",
            table: "BudgetCells",
            column: "BudgetRowId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetCells_UserId",
            table: "BudgetCells",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetGroups_UserId",
            table: "BudgetGroups",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetRows_UserId",
            table: "BudgetRows",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_TodoGroupMapping_TodoGroupId",
            table: "TodoGroupMapping",
            column: "TodoGroupId");

        migrationBuilder.CreateIndex(
            name: "IX_TodoGroupMapping_TodoId",
            table: "TodoGroupMapping",
            column: "TodoId");

        migrationBuilder.CreateIndex(
            name: "IX_TodoUserMapping_TodoId",
            table: "TodoUserMapping",
            column: "TodoId");

        migrationBuilder.CreateIndex(
            name: "IX_TodoUserMapping_UserId",
            table: "TodoUserMapping",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BudgetCells");

        migrationBuilder.DropTable(
            name: "TodoGroupMapping");

        migrationBuilder.DropTable(
            name: "TodoUserMapping");

        migrationBuilder.DropTable(
            name: "BudgetColumns");

        migrationBuilder.DropTable(
            name: "BudgetGroups");

        migrationBuilder.DropTable(
            name: "BudgetRows");

        migrationBuilder.DropTable(
            name: "TodoGroups");

        migrationBuilder.DropTable(
            name: "Todos");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
