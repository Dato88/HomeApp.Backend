using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class FixDefaultValueCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified",
                schema: "public",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 907, DateTimeKind.Utc).AddTicks(5750));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 907, DateTimeKind.Utc).AddTicks(5160));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "TodoPeople",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "TodoGroupTodos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(6110));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "TodoGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(1860));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "People",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 906, DateTimeKind.Utc).AddTicks(9500));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetRows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 906, DateTimeKind.Utc).AddTicks(4560));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 905, DateTimeKind.Utc).AddTicks(9520));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetColumns",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 905, DateTimeKind.Utc).AddTicks(5870));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetCells",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 904, DateTimeKind.Utc).AddTicks(7210));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified",
                schema: "public",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 907, DateTimeKind.Utc).AddTicks(5750),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 907, DateTimeKind.Utc).AddTicks(5160),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "TodoPeople",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(9800),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "TodoGroupTodos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(6110),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "TodoGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 908, DateTimeKind.Utc).AddTicks(1860),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "People",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 906, DateTimeKind.Utc).AddTicks(9500),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetRows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 906, DateTimeKind.Utc).AddTicks(4560),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 905, DateTimeKind.Utc).AddTicks(9520),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetColumns",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 905, DateTimeKind.Utc).AddTicks(5870),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "BudgetCells",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 27, 19, 26, 30, 904, DateTimeKind.Utc).AddTicks(7210),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");
        }
    }
}
