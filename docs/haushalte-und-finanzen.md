# Haushalte, Finanzen & E+A (Einnahmen/Ausgaben)

Technische Doku zum Haushalts- und Finanz-Modul (Stand: 2026-07). Fachliche Funktionsübersicht der API: siehe [api-funktionsuebersicht.md](./api-funktionsuebersicht.md).

## Konzept

Das Feature bildet ein Haushaltsbuch mit Plan/Ist-Vergleich ab (Vorbild: Excel-E+A):

- **Budget = Plan (SOLL).** Ein Raster pro Haushalt und Jahr: Gruppen (z. B. Wohnen, Versicherungen) → Zeilen (z. B. Miete, Strom) → Zellen (ein Betrag pro Monat).
- **Finance = Realität (IST).** Konten (Bankverbindungen mit IBAN) gehören einer Person und enthalten Buchungen (manuell oder importiert). Buchungen werden Kategorien zugeordnet.
- **Verknüpfung:** Eine Budget-Zeile kann auf eine Kategorie zeigen. Die E+A-Ansicht (`GET /Budget/eva`) aggregiert die Buchungen pro Kategorie und Monat und stellt sie den SOLL-Zellen gegenüber.

### Haushalte

- Jede Person bekommt beim ersten Login automatisch einen **persönlichen Haushalt** (Provisioning in `PersonProvisioningService`).
- Eine Person kann in **mehreren Haushalten** gleichzeitig Mitglied sein. Für gemeinsame Finanzen wird ein zusätzlicher Haushalt explizit erstellt und Partner per E-Mail/PersonId eingeladen — bestehende Haushalte bleiben davon unberührt.
- Budgets und Kategorien gehören einem Haushalt. Sichtbarkeit = Mitgliedschaft (Query-Scoping über `Household.Members.Any(m => m.PersonId == …)`, kein globaler Filter).

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
| `budget` | `budgets` | Ein Budget pro Haushalt+Jahr, unique `(household_id, year)` |
| `budget` | `budget_groups` | Gruppe mit `budget_group_type` (1=Income, 2=Expense) und `target_percent` (Zielanteil am Einkommen, z. B. 30) |
| `budget` | `budget_rows` | Zeile, optional `category_id` (Verknüpfung zur Finance-Kategorie, FK `SET NULL`) |
| `budget` | `budget_cells` | SOLL-Betrag, unique `(budget_row_id, month)`, Check `month BETWEEN 1 AND 12` |
| `finance` | `accounts` | Konto: Owner (`person_id`), `iban` (normalisiert, unique pro Person), `account_type`, `currency_code` |
| `finance` | `account_households` | Freigabe Konto↔Haushalt, unique `(account_id, household_id)` |
| `finance` | `categories` | Kategorie pro Haushalt, unique `(household_id, name)`, `category_type` (Income/Expense) |
| `finance` | `transactions` | Buchung: signierter Betrag (negativ = Ausgabe), `booking_date`, Gegenpartei, `category_id` (FK `SET NULL`), `import_hash` (unique pro Konto, gefiltert), `source` (Manual/CsvImport/CamtImport) |

Löschkaskaden: Person → Konten (→ Buchungen) und Mitgliedschaften; Haushalt → Budgets, Kategorien (Buchungen werden nur entkategorisiert), Freigaben.

IBAN: Speicherung normalisiert (ohne Whitespace, Uppercase), Validierung per ISO-13616-Mod-97 (`Domain/ValueObjects/Iban.cs`). Keine Maskierung in Responses; IBANs werden nicht geloggt.

## Migrationen

| Migration | Inhalt |
|---|---|
| `20260704…_householdSharing` | `households` + `household_members` anlegen; `budgets.person_id` → `household_id` **mit Daten-Backfill** (pro bestehender Person ein persönlicher Haushalt, Budgets werden umgehängt); Unique-Index `(household_id, year)` |
| `20260704…_financeModule` | Alle `finance.*`-Tabellen + `budget_rows.category_id` |
| `20260704…_budgetGroupTargetPercent` | `budget_groups.target_percent` (numeric(5,2), nullable) |

Der Backfill läuft als SQL innerhalb der Migration und wird in CI durch die Testcontainers-Integrationstests (`Database.Migrate()`) mit ausgeführt. Für `dotnet ef` existiert jetzt `Infrastructure/Database/HomeAppContextDesignTimeFactory.cs` (kein OAuth-Setup nötig).

## Berechtigungen (Keycloak)

- Neue Realm-Rolle **`ViewFinance`** für `AccountController`, `CategoryController`, `TransactionController` (in `homeapp-realm.json` ergänzt, devuser hat sie). Lokal greift die Realm-Änderung erst nach einem Keycloak-Volume-Reset; bestehenden echten Nutzern muss die Rolle manuell zugewiesen werden.
- `BudgetController` weiterhin `ViewBudget`; `HouseholdController` nur `[Authorize]`.
- Feingranulare Berechtigung (Owner vs. Mitglied, Mitgliedschaft) wird **datenseitig** in den `Infrastructure/Features/**`-Implementierungen erzwungen, nicht über Rollen.

## Import

- Endpunkt: `POST /Transaction/import?accountId=&format=` (Multipart-Datei, max. 5 MB, nur Owner).
- Formate: **CAMT.053-XML** (`format=camt053`, namespace-tolerant für .001.02–.08) und **Sparkassen-CSV-CAMT** (`format=sparkasse-csv`, Semikolon, `dd.MM.yy`, Dezimalkomma, UTF-8/Windows-1252-Erkennung). Ohne `format`-Parameter wird automatisch erkannt.
- **Duplikaterkennung:** SHA-256 über `kontoId|buchungstag|betrag|gegen-iban|verwendungszweck(normalisiert)` + Occurrence-Suffix für identische Buchungen innerhalb einer Datei. Der Hash ist formatunabhängig — CSV- und CAMT-Exporte desselben Kontos dedupen gegeneinander. Gefilterter Unique-Index `(account_id, import_hash)` als Race-Absicherung.
- Bekannte Grenze: identische Buchungen am selben Tag, die auf **zwei verschiedene** Dateien verteilt sind, kollidieren (Occurrence-Suffix wirkt nur innerhalb einer Datei).

## E+A-Berechnung (`GET /Budget/eva?householdId=&year=`)

- SOLL aus den Budget-Zellen; IST aus **einer** SQL-GroupBy-Query über alle Buchungen der in den Haushalt freigegebenen Konten (Kategorie × Monat); Pivot in-memory im `GetEvaQueryHandler`.
- Monats-Arrays haben fix Länge 12, Index 0 = Januar.
- **Vorzeichen-Konventionen:** Zeilen-/Gruppen-IST und die Totals werden positiv ausgewiesen (wie im Excel), `Diff = Soll − Ist`. Nur `UnassignedIst` ist **signiert** (Einnahmen positiv, Ausgaben negativ), da dort beide Richtungen in einem Topf landen.
- `UnassignedIst`/`UnassignedTransactionCount`: alle Buchungen ohne Zeilen-Zuordnung (unkategorisiert oder Kategorie keiner Zeile dieses Budgets zugeordnet) — damit „verschwindet" nichts.
- Prozente: `TargetPercent` = gepflegtes Ziel; `PlannedPercentOfIncome`/`ActualPercentOfIncome` = berechneter Anteil am Jahres-Einkommen (null bei Einkommen ≤ 0; kaufmännisch auf 2 Stellen gerundet).
- Gruppen müssen `Income` oder `Expense` sein (Validator lehnt `Unknown` ab), sonst würden ihre Werte aus den Totals fallen.

## Bewusste Design-Entscheidungen / Grenzen

- **PATCH = Full-Replace:** Die PATCH-Endpunkte ersetzen alle Felder des Objekts. Nicht mitgesendete optionale Felder (`TargetPercent`, `CategoryId`) werden auf `null` gesetzt — Clients müssen immer das vollständige Objekt senden.
- Eine Kategorie darf **pro Budget nur an einer Zeile** hängen (sonst IST-Doppelzählung). Wird in der Command-Logik erzwungen, nicht per DB-Constraint — bei exakt gleichzeitigen Updates theoretisch umgehbar.
- Breaking Change fürs Frontend: `GET/POST /Budget` benötigen jetzt `householdId` (vorher `GET /Household` aufrufen).
- Auto-Kategorisierung (Regel-Engine) ist bewusst noch nicht umgesetzt (geplant: `CategoryRule` mit Stichwort-Matching beim Import).

Fachlicher Gesamtkontext: siehe Confluence-Projektseite [Link — nachtragen].
