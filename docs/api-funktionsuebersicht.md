# API-Funktionsübersicht (fachlich)

Was die HomeApp-Backend-API aus Nutzersicht kann — gruppiert nach Modul (Stand: 2026-07). Technische Details zum Finanz-Modul: [haushalte-und-finanzen.md](./haushalte-und-finanzen.md), Authentifizierung: [authentication.md](./authentication.md).

**Grundlagen:** Alle Endpunkte erfordern einen angemeldeten Nutzer (Keycloak via BFF). Erfolgreiche Antworten sind in `Result<T>` verpackt (`{ value, isSuccess, error }`), Fehler kommen als `400 Bad Request` mit `Error`-Objekt (`code`, `description`). Jeder Nutzer sieht ausschließlich Daten, auf die er über Eigentum oder Haushalts-Mitgliedschaft Zugriff hat.

## Haushalte (`/Household`) — Rolle: angemeldet

Ein Haushalt ist der gemeinsame Rahmen für Kategorien und Kategorie-Gruppen. Jeder Nutzer hat automatisch einen persönlichen Haushalt; für Partner/Familie erstellt man zusätzliche gemeinsame Haushalte.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Meine Haushalte auflisten | `GET /Household` | Alle Haushalte inkl. Mitglieder (Name, E-Mail) |
| Haushalt erstellen | `POST /Household` | Ersteller wird automatisch Mitglied |
| Haushalt umbenennen | `PATCH /Household?householdId=&name=` | Nur Mitglieder |
| Haushalt löschen | `DELETE /Household?householdId=` | Löscht Kategorien, Kategorie-Gruppen und Freigaben mit (Kaskade); nur Mitglieder |
| Mitglied einladen | `POST /Household/member` | Per `email` **oder** `personId`; nur Mitglieder dürfen einladen |
| Mitglied entfernen / austreten | `DELETE /Household/member?householdId=&personId=` | Das letzte Mitglied kann nicht entfernt werden |

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

## Kategorie-Gruppen (`/CategoryGroup`) — Rolle: `ViewFinance`

Kategorie-Gruppen strukturieren die Kategorien eines Haushalts für den E+A-Report (z. B. „Wohnen", „Versicherungen", „Lebenshaltung", „Sparen"). Typisiert (Einnahme/Ausgabe), Name pro Haushalt eindeutig, optional mit Zielanteil am Einkommen (`targetPercent`, 0–100, z. B. 30/10/30/30-Regel).

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Gruppen auflisten | `GET /CategoryGroup?householdId=` | Nur Mitglieder |
| Anlegen / Ändern / Löschen | `POST/PATCH /CategoryGroup`, `DELETE /CategoryGroup?categoryGroupId=` | Typ muss `Income` oder `Expense` sein; beim Löschen bleiben die Kategorien bestehen (nur der Gruppen-Link wird geleert) |

## Kategorien (`/Category`) — Rolle: `ViewFinance`

Kategorien gehören einem Haushalt, sind jahresübergreifend stabil und typisiert (Einnahme/Ausgabe). Name pro Haushalt eindeutig. Optional einer Kategorie-Gruppe desselben Haushalts zugeordnet (`categoryGroupId`).

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Kategorien auflisten | `GET /Category?householdId=` | Nur Mitglieder |
| Anlegen / Ändern / Löschen | `POST/PATCH /Category`, `DELETE /Category?categoryId=` | Beim Löschen werden Buchungen nur entkategorisiert, nicht gelöscht |

**Hinweis PATCH:** ersetzt immer das vollständige Objekt — nicht mitgesendete optionale Felder (`targetPercent`, `categoryGroupId`) werden geleert.

## Buchungen (`/Transaction`) — Rolle: `ViewFinance`

Buchungen liegen auf einem Konto. Betrag ist signiert: negativ = Ausgabe, positiv = Einnahme. Schreiben darf nur der Konto-Owner; Haushaltsmitglieder freigegebener Konten dürfen lesen und kategorisieren.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| Buchungen auflisten | `GET /Transaction?accountId=&from=&to=&categoryId=&uncategorized=&counterpartyIban=&page=&pageSize=` | Paginiert (max. 200), neueste zuerst, mit `totalCount`; `counterpartyIban` filtert exakt auf die Gegen-IBAN (normalisierter Vergleich: Groß-/Kleinschreibung und Leerzeichen egal), `totalCount` zählt dann nur die Treffer |
| Manuell erfassen | `POST /Transaction` | Datum, Betrag, Gegenpartei, Verwendungszweck, optional Kategorie |
| Ändern / Löschen | `PATCH /Transaction`, `DELETE /Transaction?transactionId=` | Nur Owner |
| Kategorisieren (einzeln & Mehrfachauswahl) | `PATCH /Transaction/category` | Body: `transactionIds` (Liste, 1–500) + `categoryId` (oder `null` zum Entkategorisieren); **all-or-nothing** — bei einer ungültigen ID wird nichts gespeichert; auch für Haushaltsmitglieder; Kategorie muss zu einem Haushalt gehören, in den jedes betroffene Konto freigegeben ist |
| **Kontoauszug importieren** | `POST /Transaction/import?accountId=&format=` | Datei-Upload (max. 5 MB): CAMT.053-XML, Sparkassen-CSV, Revolut-CSV (nur abgeschlossene Umsätze; Gebühr > 0 wird als separate Buchung angelegt), Excel (`.xlsx`, gleiche Spaltennamen wie der Sparkassen-Export) oder Deutsche-Bank-Kontoauszug-PDF (positionsbasiert, best effort), Format-Autoerkennung; Duplikate werden erkannt und übersprungen (auch format­übergreifend und bei Re-Import); Antwort: `imported` / `skippedDuplicates` / `failed` + Fehlerliste |

## E+A-Report (`/Report`) — Rolle: `ViewFinance`

Der Einnahmen/Ausgaben-Report ersetzt das frühere Budget-Modul: keine manuelle Planung — alles leitet sich aus den Buchungen ab.

| Funktion | Endpunkt | Verhalten |
|---|---|---|
| **E+A-Ansicht** | `GET /Report/eva?householdIds=1&householdIds=2&year=` | Ein oder mehrere Haushalte (max. 10, Aufrufer muss überall Mitglied sein); `year<=0` → aktuelles Jahr. Antwort: Kategorie-Gruppen → Kategorien mit 12 Monatswerten + Jahressumme, ungruppierte Kategorien separat, Summen Einnahmen/Ausgaben/Differenz, Zielprozent (`targetPercent`) vs. berechneter Anteil am Einkommen (`actualPercentOfIncome`), „Unassigned"-Topf für unkategorisierte Buchungen. Konten, die in mehrere der gewählten Haushalte freigegeben sind, zählen genau einmal |

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
| Navbar-Einträge | `GET /Navigation/navbar` | Statische Menüstruktur (Dashboard, Todo, Finance, Settings) |

---

## Typischer Ablauf (gemeinsames Haushaltsbuch für zwei Personen)

1. Beide Partner melden sich einmal an (Person + persönlicher Haushalt entstehen automatisch).
2. Partner A: `POST /Household` („Familie …"), dann `POST /Household/member` mit der E-Mail von Partner B.
3. Beide legen ihre Bankverbindungen an (`POST /Account`) und geben die relevanten Konten in den gemeinsamen Haushalt frei (`POST /Account/share`).
4. Kategorie-Gruppen mit Zielprozenten anlegen (`POST /CategoryGroup`: Wohnen 30, Versicherungen 10, …), dann Kategorien anlegen und den Gruppen zuordnen (`POST /Category`: Gehalt, Miete, Einkaufen, …).
5. Buchungen manuell erfassen (`POST /Transaction`) oder Kontoauszüge importieren (`POST /Transaction/import`, auch Excel), dann kategorisieren (`PATCH /Transaction/category`, gern per Mehrfachauswahl) — das darf jedes Haushaltsmitglied.
6. `GET /Report/eva` liefert die fertige E+A: IST pro Kategorie und Monat, Gruppen-Summen, 30/10/30/30-Vergleich und den Topf nicht zugeordneter Buchungen — komplett aus den Buchungen abgeleitet, ohne manuelle Pflege.
