using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.HomeApp
{
    /// <inheritdoc />
    public partial class paymentPartners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "counterparty_name",
                schema: "finance",
                table: "transactions",
                newName: "payment_partner_name");

            migrationBuilder.RenameColumn(
                name: "counterparty_iban",
                schema: "finance",
                table: "transactions",
                newName: "payment_partner_iban");

            migrationBuilder.AddColumn<int>(
                name: "payment_partner_id",
                schema: "finance",
                table: "transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "deactivated_from",
                schema: "finance",
                table: "accounts",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "payment_partners",
                schema: "finance",
                columns: table => new
                {
                    payment_partner_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    iban = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: true),
                    linked_account_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_partners", x => x.payment_partner_id);
                    table.ForeignKey(
                        name: "fk_payment_partners_accounts_linked_account_id",
                        column: x => x.linked_account_id,
                        principalSchema: "finance",
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_payment_partners_people_person_id",
                        column: x => x.person_id,
                        principalSchema: "people",
                        principalTable: "people",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_transactions_payment_partner_id",
                schema: "finance",
                table: "transactions",
                column: "payment_partner_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_partners_linked_account_id",
                schema: "finance",
                table: "payment_partners",
                column: "linked_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_partners_person_id_iban",
                schema: "finance",
                table: "payment_partners",
                columns: new[] { "person_id", "iban" },
                unique: true,
                filter: "iban IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_payment_partners_person_id_normalized_name",
                schema: "finance",
                table: "payment_partners",
                columns: new[] { "person_id", "normalized_name" });

            migrationBuilder.CreateIndex(
                name: "ix_payment_partners_person_id_normalized_name_no_iban",
                schema: "finance",
                table: "payment_partners",
                columns: new[] { "person_id", "normalized_name" },
                unique: true,
                filter: "iban IS NULL");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_payment_partners_payment_partner_id",
                schema: "finance",
                table: "transactions",
                column: "payment_partner_id",
                principalSchema: "finance",
                principalTable: "payment_partners",
                principalColumn: "payment_partner_id",
                onDelete: ReferentialAction.SetNull);

            // Backfill: deduplicate the raw partner strings into payment_partners, scoped by the
            // account owner. IBAN identity first (one partner per owner+IBAN, display name = most
            // frequent cleaned raw name, alphabetical tiebreak), then name-only partners for the
            // remaining rows unless the matching key already exists. IBANs are re-normalized in SQL
            // because legacy rows may predate normalization-on-write. Mirrors PaymentPartnerResolver.
            migrationBuilder.Sql(
                """
                WITH tx AS (
                    SELECT a.person_id,
                           NULLIF(upper(regexp_replace(coalesce(t.payment_partner_iban, ''), '\s', '', 'g')), '') AS norm_iban,
                           NULLIF(btrim(regexp_replace(coalesce(t.payment_partner_name, ''), '\s+', ' ', 'g')), '') AS clean_name
                    FROM finance.transactions t
                    JOIN finance.accounts a ON a.account_id = t.account_id
                ),
                named AS (
                    SELECT DISTINCT ON (person_id, norm_iban) person_id, norm_iban, clean_name
                    FROM (SELECT person_id, norm_iban, clean_name, count(*) AS cnt
                          FROM tx WHERE norm_iban IS NOT NULL AND clean_name IS NOT NULL
                          GROUP BY 1, 2, 3) s
                    ORDER BY person_id, norm_iban, cnt DESC, clean_name
                )
                INSERT INTO finance.payment_partners
                    (person_id, display_name, normalized_name, iban, created_at, created_by_id)
                SELECT i.person_id,
                       coalesce(n.clean_name, i.norm_iban),
                       coalesce(upper(n.clean_name), i.norm_iban),
                       i.norm_iban, now(), i.person_id
                FROM (SELECT DISTINCT person_id, norm_iban FROM tx WHERE norm_iban IS NOT NULL) i
                LEFT JOIN named n ON n.person_id = i.person_id AND n.norm_iban = i.norm_iban;

                WITH tx AS (
                    SELECT a.person_id,
                           NULLIF(upper(regexp_replace(coalesce(t.payment_partner_iban, ''), '\s', '', 'g')), '') AS norm_iban,
                           NULLIF(btrim(regexp_replace(coalesce(t.payment_partner_name, ''), '\s+', ' ', 'g')), '') AS clean_name
                    FROM finance.transactions t
                    JOIN finance.accounts a ON a.account_id = t.account_id
                ),
                named AS (
                    SELECT DISTINCT ON (person_id, upper(clean_name)) person_id, clean_name, upper(clean_name) AS norm_name
                    FROM (SELECT person_id, clean_name, count(*) AS cnt
                          FROM tx WHERE norm_iban IS NULL AND clean_name IS NOT NULL
                          GROUP BY 1, 2) s
                    ORDER BY person_id, upper(clean_name), cnt DESC, clean_name
                )
                INSERT INTO finance.payment_partners
                    (person_id, display_name, normalized_name, iban, created_at, created_by_id)
                SELECT n.person_id, n.clean_name, n.norm_name, NULL, now(), n.person_id
                FROM named n
                WHERE NOT EXISTS (SELECT 1 FROM finance.payment_partners p
                                  WHERE p.person_id = n.person_id AND p.normalized_name = n.norm_name);

                UPDATE finance.transactions t
                SET payment_partner_id = p.payment_partner_id
                FROM finance.accounts a, finance.payment_partners p
                WHERE a.account_id = t.account_id
                  AND NULLIF(upper(regexp_replace(coalesce(t.payment_partner_iban, ''), '\s', '', 'g')), '') IS NOT NULL
                  AND p.person_id = a.person_id
                  AND p.iban = upper(regexp_replace(t.payment_partner_iban, '\s', '', 'g'));

                UPDATE finance.transactions t
                SET payment_partner_id = p.payment_partner_id
                FROM finance.accounts a,
                     (SELECT DISTINCT ON (pp.person_id, pp.normalized_name)
                             pp.person_id, pp.normalized_name, pp.payment_partner_id
                      FROM finance.payment_partners pp
                      ORDER BY pp.person_id, pp.normalized_name, pp.payment_partner_id) p
                WHERE a.account_id = t.account_id
                  AND p.person_id = a.person_id
                  AND p.normalized_name = upper(btrim(regexp_replace(t.payment_partner_name, '\s+', ' ', 'g')))
                  AND NULLIF(upper(regexp_replace(coalesce(t.payment_partner_iban, ''), '\s', '', 'g')), '') IS NULL
                  AND NULLIF(btrim(regexp_replace(coalesce(t.payment_partner_name, ''), '\s+', ' ', 'g')), '') IS NOT NULL;

                UPDATE finance.payment_partners p
                SET linked_account_id = a.account_id
                FROM finance.accounts a
                WHERE a.person_id = p.person_id
                  AND p.iban IS NOT NULL
                  AND upper(regexp_replace(coalesce(a.iban, ''), '\s', '', 'g')) = p.iban;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transactions_payment_partners_payment_partner_id",
                schema: "finance",
                table: "transactions");

            migrationBuilder.DropTable(
                name: "payment_partners",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "ix_transactions_payment_partner_id",
                schema: "finance",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "payment_partner_id",
                schema: "finance",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "deactivated_from",
                schema: "finance",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "payment_partner_name",
                schema: "finance",
                table: "transactions",
                newName: "counterparty_name");

            migrationBuilder.RenameColumn(
                name: "payment_partner_iban",
                schema: "finance",
                table: "transactions",
                newName: "counterparty_iban");
        }
    }
}
