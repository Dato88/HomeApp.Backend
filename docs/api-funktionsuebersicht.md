# API-Funktionsübersicht (fachlich)

Was die HomeApp-Backend-API aus Nutzersicht kann — gruppiert nach Modul (Stand: 2026-07). Technische Details zum Finanz-Modul: [haushalte-und-finanzen.md](./haushalte-und-finanzen.md), Authentifizierung: [authentication.md](./authentication.md).

**Grundlagen:** Alle Endpunkte erfordern einen angemeldeten Nutzer (Keycloak via BFF). Erfolgreiche Antworten sind in `Result<T>` verpackt (`{ value, isSuccess, error }`), Fehler kommen als `400 Bad Request` mit `Error`-Objekt (`code`, `description`). Jeder Nutzer sieht ausschließlich Daten, auf die er über Eigentum oder Haushalts-Mitgliedschaft Zugriff hat.

## Haushalte (`/Household`) — Rolle: angemeldet

Ein Haushalt ist der gemeinsame Rahmen für Budgets und Kategorien. Jeder Nutzer hat automatisch einen persönlichen Haushalt; für Partner/Familie erstellt man zusätzliche gemeinsame Haushalte.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Meine Haushalte auflisten | `GET /Household` | Alle Haushalte inkl. Mitglieder (Name, E-Mail) |
| Haushalt erstellen | `POST /Household` | Ersteller wird automatisch Mitglied |
| Haushalt umbenennen | `PATCH /Household?householdId=&name=` | Nur Mitglieder |
| Haushalt löschen | `DELETE /Household?householdId=` | Löscht Budgets, Kategorien und Freigaben mit (Kaskade); nur Mitglieder |
| Mitglied einladen | `POST /Household/member` | Per `email` **oder** `personId`; nur Mitglieder dürfen einladen |
| Mitglied entfernen / austreten | `DELETE /Household/member?householdId=&personId=` | Das letzte Mitglied kann nicht entfernt werden |

## Budget = Plan/SOLL (`/Budget`) — Rolle: `ViewBudget`

Ein Budget gehört einem Haushalt und einem Jahr (ein Budget pro Haushalt+Jahr). Struktur: Gruppen (Einnahmen/Ausgaben, z. B. „Wohnen") → Zeilen (z. B. „Miete") → Monats-Zellen (SOLL-Beträge, 0 und negativ erlaubt).

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Budget lesen | `GET /Budget?householdId=&year=` | Komplettes Raster (Gruppen/Zeilen/Zellen als flache Listen); `204` wenn keins existiert; `year<=0` → aktuelles Jahr |
| Budget anlegen | `POST /Budget?householdId=&year=` | Fehler, wenn das Jahr im Haushalt schon existiert |
| Budget-Jahr ändern | `PATCH /Budget?budgetId=&year=` | |
| Budget löschen | `DELETE /Budget?budgetId=` | Löscht Gruppen/Zeilen/Zellen mit |
| Gruppe anlegen/ändern/löschen | `POST/PATCH /Budget/group`, `DELETE /Budget/group?budgetGroupId=` | Typ muss `Income` oder `Expense` sein; optional `targetPercent` (Zielanteil am Einkommen, 0–100, z. B. 30/10/30/30-Regel) |
| Zeile anlegen/ändern/löschen | `POST/PATCH /Budget/row`, `DELETE /Budget/row?budgetRowId=` | Optional `categoryId` → verknüpft die Zeile mit einer Finance-Kategorie (liefert das IST); eine Kategorie max. an einer Zeile pro Budget |
| Zelle anlegen/ändern/löschen | `POST/PATCH /Budget/cell`, `DELETE /Budget/cell?budgetCellId=` | Ein SOLL-Betrag pro Zeile und Monat (1–12) |
| **E+A-Ansicht (SOLL vs. IST)** | `GET /Budget/eva?householdId=&year=` | Das Excel-Raster als JSON: pro Gruppe/Zeile SOLL, IST (aus Buchungen), Differenz je Monat + Jahreswerte; Zielprozent vs. berechneter Anteil am Einkommen; Summen Einnahmen/Ausgaben/Differenz; „Unassigned"-Topf für nicht zugeordnete Buchungen |

**Hinweis PATCH:** ersetzt immer das vollständige Objekt — nicht mitgesendete optionale Felder (`targetPercent`, `categoryId`) werden geleert.

## Konten / Bankverbindungen (`/Account`) — Rolle: `ViewFinance`

Ein Konto (Girokonto, Sparkonto, Kreditkarte, Depot, Bargeld …) gehört dem Nutzer, der es anlegt. Beliebig viele Konten pro Nutzer; IBAN optional (z. B. Bargeld), wird validiert (Mod-97) und normalisiert gespeichert.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Konten auflisten | `GET /Account` | Eigene Konten + in meine Haushalte freigegebene; mit `isOwner` und `sharedHouseholdIds` |
| Konto anlegen | `POST /Account` | Name, IBAN?, BIC?, Typ, Währung (Default EUR); optional `householdIds` für sofortige Freigabe |
| Konto ändern | `PATCH /Account` | Nur Owner |
| Konto löschen | `DELETE /Account?accountId=` | Nur Owner; löscht alle Buchungen mit |
| In Haushalt freigeben | `POST /Account/share` | Nur Owner, nur in Haushalte mit eigener Mitgliedschaft; erst danach zählen die Buchungen in die E+A dieses Haushalts und Mitglieder sehen das Konto |
| Freigabe entziehen | `DELETE /Account/share?accountId=&householdId=` | Nur Owner |

## Kategorien (`/Category`) — Rolle: `ViewFinance`

Kategorien gehören einem Haushalt, sind jahresübergreifend stabil (einmal kategorisierte Buchungen zählen in jedes Jahres-Budget) und typisiert (Einnahme/Ausgabe). Name pro Haushalt eindeutig.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Kategorien auflisten | `GET /Category?householdId=` | Nur Mitglieder |
| Anlegen / Ändern / Löschen | `POST/PATCH /Category`, `DELETE /Category?categoryId=` | Beim Löschen werden Buchungen nur entkategorisiert, nicht gelöscht |

## Buchungen (`/Transaction`) — Rolle: `ViewFinance`

Buchungen liegen auf einem Konto. Betrag ist signiert: negativ = Ausgabe, positiv = Einnahme. Schreiben darf nur der Konto-Owner; Haushaltsmitglieder freigegebener Konten dürfen lesen und kategorisieren.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Buchungen auflisten | `GET /Transaction?accountId=&from=&to=&categoryId=&uncategorized=&page=&pageSize=` | Paginiert (max. 200), neueste zuerst, mit `totalCount` |
| Manuell erfassen | `POST /Transaction` | Datum, Betrag, Gegenpartei, Verwendungszweck, optional Kategorie |
| Ändern / Löschen | `PATCH /Transaction`, `DELETE /Transaction?transactionId=` | Nur Owner |
| Kategorisieren | `PATCH /Transaction/category` | Auch für Haushaltsmitglieder; Kategorie muss zu einem Haushalt gehören, in den das Konto freigegeben ist |
| **Kontoauszug importieren** | `POST /Transaction/import?accountId=&format=` | Datei-Upload (max. 5 MB): CAMT.053-XML oder Sparkassen-CSV, Format-Autoerkennung; Duplikate werden erkannt und übersprungen (auch format­übergreifend und bei Re-Import); Antwort: `imported` / `skippedDuplicates` / `failed` + Fehlerliste |

## Personen (`/Person`) — Rolle: angemeldet

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Eigenes Profil lesen | `GET /Person/person` | Person wird beim ersten Login automatisch aus dem Keycloak-Token angelegt (inkl. persönlichem Haushalt) |

## Todos (`/Todo`) — Rolle: `ViewTodo`

| Funktion | Endpunkt |
|---|---|
| Eigene Todos auflisten | `GET /Todo/todos` |
| Einzelnes Todo lesen | `GET /Todo/todo` |
| Anlegen / Ändern / Löschen | `POST/PATCH/DELETE /Todo/todo` |

## Navigation (`/Navigation`) — Rolle: angemeldet

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Navbar-Einträge | `GET /Navigation/navbar` | Statische Menüstruktur (Dashboard, Todo, Budget, Finance, Settings) |

---

## Typischer Ablauf (gemeinsames Haushaltsbuch für zwei Personen)

1. Beide Partner melden sich einmal an (Person + persönlicher Haushalt entstehen automatisch).
2. Partner A: `POST /Household` („Familie …"), dann `POST /Household/member` mit der E-Mail von Partner B.
3. Beide legen ihre Bankverbindungen an (`POST /Account`) und geben die relevanten Konten in den gemeinsamen Haushalt frei (`POST /Account/share`).
4. Kategorien im gemeinsamen Haushalt anlegen (`POST /Category`: Gehalt, Miete, Einkaufen, …).
5. Buchungen manuell erfassen (`POST /Transaction`) oder Kontoauszüge importieren (`POST /Transaction/import`), dann kategorisieren (`PATCH /Transaction/category`) — das darf jedes Haushaltsmitglied.
6. Budget fürs Jahr anlegen (`POST /Budget`), Gruppen mit Zielprozenten und Zeilen mit SOLL-Werten pflegen, Zeilen über `categoryId` mit den Kategorien verknüpfen.
7. `GET /Budget/eva` liefert die fertige E+A: SOLL vs. IST pro Monat, Gruppen-Summen, 30/10/30/30-Vergleich und den Topf nicht zugeordneter Buchungen.
