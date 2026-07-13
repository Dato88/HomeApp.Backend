# Haushalte, Finanzen & E+A (Einnahmen/Ausgaben)

Technische Doku zum Haushalts- und Finanz-Modul (Stand: 2026-07). Fachliche Funktionsübersicht der API: siehe [api-funktionsuebersicht.md](./api-funktionsuebersicht.md). Zur Entfernung des Budget-Moduls: siehe [ADR 0002](./adr/0002-budget-modul-entfernt-eva-aus-transaktionen.md).

## Konzept

Das Feature bildet ein Haushaltsbuch ab, das sich **vollständig aus den Buchungen ableitet** (Vorbild: Excel-E+A, aber ohne manuelle Pflege):

- **Finance = Realität (IST).** Konten (Bankverbindungen mit IBAN) gehören einer Person und enthalten Buchungen (manuell oder importiert). Buchungen werden Kategorien zugeordnet — einzeln oder als Mehrfachauswahl.
- **Kategorie-Gruppen** strukturieren die Kategorien pro Haushalt (z. B. Wohnen, Versicherungen, Lebenshaltung, Sparen) und tragen optional einen Zielanteil am Einkommen (`target_percent`, z. B. 30/10/30/30-Regel).
- **E+A-Report:** `GET /Report/eva` aggregiert die Buchungen pro Kategorie und Monat und rollt sie über die Gruppen hoch — es gibt **keine manuelle SOLL-Planung** mehr (das frühere Budget-Modul mit Gruppen/Zeilen/Zellen wurde entfernt).

### Haushalte

- Jede Person bekommt beim ersten Login automatisch einen **persönlichen Haushalt** (Provisioning in `PersonProvisioningService`).
- Eine Person kann in **mehreren Haushalten** gleichzeitig Mitglied sein. Für gemeinsame Finanzen wird ein zusätzlicher Haushalt explizit erstellt und Partner per E-Mail/PersonId eingeladen — bestehende Haushalte bleiben davon unberührt.
- Kategorien und Kategorie-Gruppen gehören einem Haushalt. Sichtbarkeit = Mitgliedschaft (Query-Scoping über `Household.Members.Any(m => m.PersonId == …)`, kein globaler Filter).

### Konten & Freigabe

- Ein Konto gehört genau **einer Person** (Owner). Über die Freigabe (`AccountHousehold`-Join) wird es in einen oder mehrere Haushalte „eingeblendet".
- **Owner-only:** Konto ändern/löschen, Buchungen anlegen/ändern/löschen, Import, Freigabe erteilen/entziehen.
- **Haushaltsmitglieder** (bei freigegebenen Konten): lesen und Buchungen kategorisieren.
- Buchungen zählen nur in die E+A eines Haushalts, wenn das Konto **dorthin freigegeben** ist — auch beim eigenen persönlichen Haushalt ist die Freigabe explizit.

## Datenmodell

| Schema | Tabelle | Inhalt |
|---|---|---|
| `people` | `households` | Haushalt (`name`) |
| `people` | `household_members` | Mitgliedschaft, unique `(household_id, person_id)` |
| `finance` | `accounts` | Konto: Owner (`person_id`), `iban` (normalisiert, unique pro Person), `account_type`, `currency_code` |
| `finance` | `account_households` | Freigabe Konto↔Haushalt, unique `(account_id, household_id)` |
| `finance` | `category_groups` | Kategorie-Gruppe pro Haushalt, unique `(household_id, name)`, `category_group_type` (1=Income, 2=Expense) und `target_percent` (Zielanteil am Einkommen, numeric(5,2), nullable) |
| `finance` | `categories` | Kategorie pro Haushalt, unique `(household_id, name)`, `category_type` (Income/Expense), optional `category_group_id` (FK `SET NULL`) |
| `finance` | `transactions` | Buchung: signierter Betrag (negativ = Ausgabe), `booking_date`, Gegenpartei, `category_id` (FK `SET NULL`), `import_hash` (unique pro Konto, gefiltert), `source` (Manual/CsvImport/CamtImport/XlsxImport/PdfImport) |

Löschkaskaden: Person → Konten (→ Buchungen) und Mitgliedschaften; Haushalt → Kategorien und Kategorie-Gruppen (Buchungen werden nur entkategorisiert), Freigaben. Beim Löschen einer Kategorie-Gruppe bleiben die Kategorien bestehen (nur der Gruppen-Link wird geleert).

IBAN: Speicherung normalisiert (ohne Whitespace, Uppercase), Validierung per ISO-13616-Mod-97 (`Domain/ValueObjects/Iban.cs`). Keine Maskierung in Responses; IBANs werden nicht geloggt.

## Migrationen

| Migration | Inhalt |
|---|---|
| `20260704…_householdSharing` | `households` + `household_members` anlegen; `budgets.person_id` → `household_id` **mit Daten-Backfill** (pro bestehender Person ein persönlicher Haushalt, Budgets werden umgehängt) |
| `20260704…_financeModule` | Alle `finance.*`-Tabellen + `budget_rows.category_id` |
| `20260704…_budgetGroupTargetPercent` | `budget_groups.target_percent` (numeric(5,2), nullable) |
| `20260712…_categoryGroupsReplaceBudget` | `finance.category_groups` + `categories.category_group_id` anlegen; **Daten-Backfill** (pro Haushalt werden die Gruppen des jüngsten Budget-Jahres übernommen — Name, Typ, `target_percent` — und die Kategorien über die bisherigen Budget-Zeilen verlinkt); danach werden `budget_cells`, `budget_rows`, `budget_groups`, `budgets` und das Schema `budget` **gelöscht**. Die `Down()`-Migration stellt nur leere Budget-Tabellen wieder her (verlustbehaftet, best effort). |

Der Backfill läuft als SQL innerhalb der Migration und wird in CI durch die Testcontainers-Integrationstests (`Database.Migrate()`) mit ausgeführt. Für `dotnet ef` existiert `Infrastructure/Database/HomeAppContextDesignTimeFactory.cs` (kein OAuth-Setup nötig).

## Berechtigungen (Keycloak)

- Realm-Rolle **`ViewFinance`** für `AccountController`, `CategoryController`, `CategoryGroupController`, `TransactionController` und `ReportController`; `HouseholdController` nur `[Authorize]`.
- Die Rolle **`ViewBudget` entfällt** (kein Controller referenziert sie mehr) und wurde aus `homeapp-realm.json` entfernt. Lokal greift die Realm-Änderung erst nach einem Keycloak-Volume-Reset; auf echten Instanzen ist die verwaiste Rolle harmlos und kann manuell gelöscht werden.
- Feingranulare Berechtigung (Owner vs. Mitglied, Mitgliedschaft) wird **datenseitig** in den `Infrastructure/Features/**`-Implementierungen erzwungen, nicht über Rollen.

## Import

- Endpunkt: `POST /Transaction/import?accountId=&format=` (Multipart-Datei, max. 5 MB, nur Owner; erlaubte Endungen `.csv`, `.xml`, `.txt`, `.xlsx`, `.pdf`).
- Formate (ohne `format`-Parameter automatische Erkennung über Dateiendung/Header/Magic Bytes):
  - **CAMT.053-XML** (`format=camt053`, namespace-tolerant für .001.02–.08)
  - **Sparkassen-CSV-CAMT** (`format=sparkasse-csv`, Semikolon, `dd.MM.yy`, Dezimalkomma, UTF-8/Windows-1252-Erkennung)
  - **Revolut-CSV** (`format=revolut-csv`, der App-Export `account-statement_…​.csv`): Komma-getrennt, deutsche **und** englische Header, Timestamps `yyyy-MM-dd HH:mm:ss` (Buchungstag = Abschluss-, Valuta = Beginn-Datum), Dezimalpunkt. Nur Zeilen mit Status `ABGESCHLOSSEN`/`COMPLETED` (inkl. interner Transfers wie „An EUR Tagesgeld" — die hält man per Kategorie „Umbuchung" aus der E+A); `PENDING`/`REVERTED` wird übersprungen. Eine **Gebühr > 0 erzeugt eine zweite, separat kategorisierbare Buchung** (negativ, Verwendungszweck `Gebühr: …`).
  - **Excel** (`format=xlsx`, via ClosedXML: erstes Worksheet, Header-Zeile mit denselben deutschen Spaltennamen wie der Sparkassen-CSV-Export — Pflicht: `Buchungstag`, `Betrag`; Zellen dürfen native Excel-Datums-/Zahlenwerte oder deutsch formatierter Text sein)
  - **Deutsche-Bank-Kontoauszug-PDF** (`format=deutsche-bank-pdf`, via PdfPig): PDFs tragen keine Datenstruktur, daher **positionsbasiertes** Parsen — die Kopfzeilen-Wörter Buchung/Valuta/Vorgang/Soll/Haben definieren die Spalten, daraus werden Buchungen inkl. mehrzeiligem Verwendungszweck und Gegen-IBAN rekonstruiert; SEPA-Metadaten (Gläubiger-ID, Mand-ID, RCUR, …) werden ausgefiltert, „Neuer Saldo" beendet das Parsen. **Best effort:** bewusst auf das private Kontoauszug-Layout beschränkt; Einträge, die ein Layout-Wechsel bricht, landen in der Fehlerliste der Antwort statt im Import.
- **Duplikaterkennung:** SHA-256 über `kontoId|buchungstag|betrag|gegen-iban|verwendungszweck(normalisiert)` + Occurrence-Suffix für identische Buchungen innerhalb einer Datei. Der Hash ist formatunabhängig — CSV-, CAMT-, XLSX- und PDF-Importe desselben Kontos dedupen gegeneinander. Gefilterter Unique-Index `(account_id, import_hash)` als Race-Absicherung.
- Bekannte Grenze: identische Buchungen am selben Tag, die auf **zwei verschiedene** Dateien verteilt sind, kollidieren (Occurrence-Suffix wirkt nur innerhalb einer Datei).

## Kategorisierung (einzeln & Mehrfachauswahl)

- `PATCH /Transaction/category` nimmt eine **Liste** von `transactionIds` (1–500) plus `categoryId` (oder `null` zum Entkategorisieren) — damit lassen sich z. B. alle wiederkehrenden Buchungen einer Auswahl in einem Rutsch gruppieren.
- **All-or-nothing:** Ist auch nur eine ID unbekannt/unsichtbar oder die Kategorie für eines der betroffenen Konten ungültig, schlägt der ganze Batch fehl und nichts wird gespeichert (ein `SaveChangesAsync`).
- Erlaubt für den Owner und für Mitglieder von Haushalten, in die das Konto freigegeben ist; die Kategorie muss zu einem Haushalt gehören, in den **jedes** betroffene Konto freigegeben ist.

## E+A-Report (`GET /Report/eva?householdIds=…&year=`)

- Rein transaktionsbasiert: IST aus **einer** SQL-GroupBy-Query über alle Buchungen der in die gewählten Haushalte freigegebenen Konten (Kategorie × Monat); Pivot in-memory im `GetEvaReportQueryHandler`. Kein SOLL, keine manuelle Pflege.
- **Ein oder mehrere Haushalte:** `householdIds` wird wiederholt übergeben (`?householdIds=1&householdIds=2`, max. 10). Der Aufrufer muss Mitglied **aller** angefragten Haushalte sein, sonst Fehler (kein stilles Teilergebnis). Ein Konto, das in mehrere der gewählten Haushalte freigegeben ist, zählt **genau einmal** (EXISTS-Filter statt Join).
- Struktur der Antwort: `groups` (Kategorie-Gruppen mit ihren Kategorien) → je 12 Monatswerte (Index 0 = Januar) + Jahressumme; `ungroupedCategories` für Kategorien ohne Gruppe; `totals` (Einnahmen/Ausgaben/Differenz je Monat + Jahr); `unassignedIst` + `unassignedTransactionCount`.
- **Vorzeichen-Konventionen:** Gruppen-Werte werden positiv ausgewiesen (Expense-Gruppen negieren die Rohsummen, wie im Excel). Ungruppierte Kategorien flippen nach ihrem eigenen `category_type`; Typ `Unknown` bleibt signiert und wird in den Totals monatsweise nach Vorzeichen einsortiert. Nur `unassignedIst` ist **signiert**, da dort beide Richtungen in einem Topf landen.
- `unassignedIst`/`unassignedTransactionCount`: unkategorisierte Buchungen **plus** Buchungen mit einer Kategorie eines nicht angefragten Haushalts — damit „verschwindet" nichts.
- Prozente: `targetPercent` = gepflegtes Ziel je Gruppe (Passthrough); `actualPercentOfIncome` = berechneter Anteil des Gruppen-Jahres-IST am Jahres-Einkommen (null bei Einkommen ≤ 0; kaufmännisch auf 2 Stellen gerundet).
- Gruppen müssen `Income` oder `Expense` sein (Validator lehnt `Unknown` ab), sonst würden ihre Werte aus den Totals fallen.

## Bewusste Design-Entscheidungen / Grenzen

- **PATCH = Full-Replace:** Die PATCH-Endpunkte ersetzen alle Felder des Objekts. Nicht mitgesendete optionale Felder (`targetPercent`, `categoryGroupId`) werden auf `null` gesetzt — Clients müssen immer das vollständige Objekt senden.
- Es gibt **keine Validierung**, dass der `category_type` einer Kategorie zum Typ ihrer Gruppe passt (der Report flippt grupppierte Kategorien nach dem Gruppen-Typ) — bewusst tolerant, damit der Migrations-Backfill nicht scheitern kann.
- Breaking Changes fürs Frontend (gebündelt): `/Budget/*` entfällt komplett (E+A jetzt `GET /Report/eva`), `PATCH /Transaction/category` erwartet eine `transactionIds`-Liste, Category-DTO/Requests haben `categoryGroupId`, die Navbar verliert „Budget", Rolle `ViewBudget` entfällt.
- Auto-Kategorisierung (Regel-Engine) ist bewusst noch nicht umgesetzt (geplant: `CategoryRule` mit Stichwort-Matching beim Import).

Fachlicher Gesamtkontext: siehe Confluence-Projektseite [Link — nachtragen].
