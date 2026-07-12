# ADR 0002: Budget-Modul entfernt — E+A rein aus Transaktionen

**Status:** akzeptiert · **Datum:** 2026-07

## Kontext

Das Budget-Modul (`Budget → BudgetGroup → BudgetRow → BudgetCell`) war ein Nachbau des Excel-Haushaltsbuchs mit manueller SOLL-Planung: pro Haushalt und Jahr musste ein Budget angelegt, Gruppen/Zeilen/Monats-Zellen von Hand gepflegt und Zeilen einzeln mit Kategorien verknüpft werden, bevor die E+A-Ansicht (`GET /Budget/eva`) etwas anzeigte. Die eigentliche Anforderung war aber eine Einnahmen/Ausgaben-Auswertung, die sich **vollständig aus den Buchungen ableitet** — das Excel-Bild war als Anzeige-Vorlage gedacht, nicht als Pflege-Modell. Fachlich ist das Gewünschte kein Budget (= Plan), sondern ein Cashflow-Report; der Name führte in die Irre.

## Geprüfte Alternativen

- **Budget-Modul behalten, nur Lücken schließen** — verworfen: Der Pflegeaufwand (Jahr anlegen, Gruppen/Zeilen/Zellen bauen, Kategorien verknüpfen) bleibt, obwohl niemand SOLL-Werte planen will; vier Entitäten und ein Controller nur für ein ungenutztes Konzept.
- **Hybrid: transaktionsbasierter Report + optionale SOLL-Planung** — verworfen: Doppelte Komplexität für ein Feature, das explizit nicht gebraucht wird; kann bei Bedarf später als leichtgewichtige Ergänzung (SOLL-Betrag pro Kategorie/Monat) nachgerüstet werden.
- **Budget-Modul entfernen, Report aus Transaktionen** — gewählt.

## Entscheidung

Das Budget-Modul entfällt komplett (Entitäten, Controller, Schema `budget`, Rolle `ViewBudget`). Die Gruppierungsstruktur wandert als **`CategoryGroup`** ins Finance-Modul: eine Kategorie-Gruppe pro Haushalt mit Typ (Income/Expense) und optionalem `target_percent` (30/10/30/30-Regel); Kategorien erhalten ein optionales `category_group_id`. Der E+A-Report lebt als **`GET /Report/eva`** im Finance-Modul (Rolle `ViewFinance`), unterstützt einen oder mehrere Haushalte und berechnet alles aus den Buchungen. Die Migration `categoryGroupsReplaceBudget` übernimmt per SQL-Backfill die Gruppen des jüngsten Budget-Jahres je Haushalt (inkl. Zielprozente und Kategorie-Verlinkung über die Budget-Zeilen) und droppt danach die Budget-Tabellen. Ergebnis: zwei Module (Finance, Household) statt drei.

## Konsequenzen

- Kein manueller Pflegeaufwand mehr: Der Report ist sofort vollständig, sobald Buchungen kategorisiert sind; SOLL/IST-Vergleich reduziert sich auf Zielprozente je Gruppe.
- Breaking Changes fürs Frontend: `/Budget/*` entfällt (E+A jetzt `GET /Report/eva`), `PATCH /Transaction/category` erwartet eine `transactionIds`-Liste, Category-Requests/DTOs haben `categoryGroupId`, Navbar-Eintrag „Budget" und Rolle `ViewBudget` entfallen.
- Die `Down()`-Migration ist **verlustbehaftet**: geplante SOLL-Zellen und die Budget-Struktur sind nach dem Upgrade unwiederbringlich (nur leere Budget-Tabellen werden wiederhergestellt) — akzeptiert, da die SOLL-Daten fachlich aufgegeben werden.
- Historische Budgets mehrerer Jahre kollabieren beim Backfill auf die Gruppen des **jüngsten** Jahres (Kategorie-Gruppen sind jahresunabhängig).
