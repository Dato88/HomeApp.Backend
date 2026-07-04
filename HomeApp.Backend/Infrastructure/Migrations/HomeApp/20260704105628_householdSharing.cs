using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class householdSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_budgets_people_person_id",
                schema: "budget",
                table: "budgets");

            migrationBuilder.RenameColumn(
                name: "person_id",
                schema: "budget",
                table: "budgets",
                newName: "household_id");

            migrationBuilder.RenameIndex(
                name: "ix_budgets_person_id_year",
                schema: "budget",
                table: "budgets",
                newName: "ix_budgets_household_id_year");

            migrationBuilder.CreateTable(
                name: "households",
                schema: "people",
                columns: table => new
                {
                    household_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_households", x => x.household_id);
                });

            migrationBuilder.CreateTable(
                name: "household_members",
                schema: "people",
                columns: table => new
                {
                    household_member_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    household_id = table.Column<int>(type: "integer", nullable: false),
                    person_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_household_members", x => x.household_member_id);
                    table.ForeignKey(
                        name: "fk_household_members_households_household_id",
                        column: x => x.household_id,
                        principalSchema: "people",
                        principalTable: "households",
                        principalColumn: "household_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_household_members_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_household_members_household_id_person_id",
                schema: "people",
                table: "household_members",
                columns: new[] { "household_id", "person_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_household_members_person_id",
                schema: "people",
                table: "household_members",
                column: "person_id");

            // Backfill: one personal household per existing person; budgets.household_id still
            // holds the old person ids after the rename above and is remapped to the new household ids.
            migrationBuilder.Sql(
                """
                INSERT INTO people.households (name, created_at, created_by_id)
                SELECT p.first_name || ' ' || p.last_name, now(), p.person_id
                FROM people.people p;

                INSERT INTO people.household_members (household_id, person_id, created_at, created_by_id)
                SELECT h.household_id, h.created_by_id, now(), h.created_by_id
                FROM people.households h;

                UPDATE budget.budgets b
                SET household_id = hm.household_id
                FROM people.household_members hm
                WHERE hm.person_id = b.household_id;
                """);

            migrationBuilder.AddForeignKey(
                name: "fk_budgets_households_household_id",
                schema: "budget",
                table: "budgets",
                column: "household_id",
                principalSchema: "people",
                principalTable: "households",
                principalColumn: "household_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_budgets_households_household_id",
                schema: "budget",
                table: "budgets");

            // Best effort: map budgets back to the person who created the household
            migrationBuilder.Sql(
                """
                UPDATE budget.budgets b
                SET household_id = h.created_by_id
                FROM people.households h
                WHERE h.household_id = b.household_id;
                """);

            migrationBuilder.DropTable(
                name: "household_members",
                schema: "people");

            migrationBuilder.DropTable(
                name: "households",
                schema: "people");

            migrationBuilder.RenameColumn(
                name: "household_id",
                schema: "budget",
                table: "budgets",
                newName: "person_id");

            migrationBuilder.RenameIndex(
                name: "ix_budgets_household_id_year",
                schema: "budget",
                table: "budgets",
                newName: "ix_budgets_person_id_year");

            migrationBuilder.AddForeignKey(
                name: "fk_budgets_people_person_id",
                schema: "budget",
                table: "budgets",
                column: "person_id",
                principalSchema: "people",
                principalTable: "people",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
