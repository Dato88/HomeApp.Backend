using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateoftodotables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoGroupMapping");

            migrationBuilder.DropTable(
                name: "TodoUserMapping");

            migrationBuilder.RenameColumn(
                name: "TodoPriority",
                table: "Todos",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "TodoName",
                table: "Todos",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TodoExecutionDate",
                table: "Todos",
                newName: "ExecutionDate");

            migrationBuilder.RenameColumn(
                name: "TodoDone",
                table: "Todos",
                newName: "Done");

            migrationBuilder.RenameColumn(
                name: "TodoGroupName",
                table: "TodoGroups",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "TodoGroupTodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TodoId = table.Column<int>(type: "integer", nullable: false),
                    TodoGroupId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoGroupTodos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoGroupTodos_TodoGroups_TodoGroupId",
                        column: x => x.TodoGroupId,
                        principalTable: "TodoGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoGroupTodos_Todos_TodoId",
                        column: x => x.TodoId,
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoPeople",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    TodoId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoPeople", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoPeople_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoPeople_Todos_TodoId",
                        column: x => x.TodoId,
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoGroupTodos_TodoGroupId",
                table: "TodoGroupTodos",
                column: "TodoGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoGroupTodos_TodoId",
                table: "TodoGroupTodos",
                column: "TodoId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoPeople_PersonId",
                table: "TodoPeople",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoPeople_TodoId",
                table: "TodoPeople",
                column: "TodoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoGroupTodos");

            migrationBuilder.DropTable(
                name: "TodoPeople");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Todos",
                newName: "TodoPriority");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Todos",
                newName: "TodoName");

            migrationBuilder.RenameColumn(
                name: "ExecutionDate",
                table: "Todos",
                newName: "TodoExecutionDate");

            migrationBuilder.RenameColumn(
                name: "Done",
                table: "Todos",
                newName: "TodoDone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TodoGroups",
                newName: "TodoGroupName");

            migrationBuilder.CreateTable(
                name: "TodoGroupMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TodoGroupId = table.Column<int>(type: "integer", nullable: false),
                    TodoId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
                name: "TodoUserMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    TodoId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoUserMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoUserMapping_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TodoUserMapping_Todos_TodoId",
                        column: x => x.TodoId,
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoGroupMapping_TodoGroupId",
                table: "TodoGroupMapping",
                column: "TodoGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoGroupMapping_TodoId",
                table: "TodoGroupMapping",
                column: "TodoId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoUserMapping_PersonId",
                table: "TodoUserMapping",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoUserMapping_TodoId",
                table: "TodoUserMapping",
                column: "TodoId");
        }
    }
}
