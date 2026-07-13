# ADR 0003: Zahlungspartner als eigene Entität (PaymentPartner) mit IBAN-first-Matching

**Status:** akzeptiert · **Datum:** 2026-07

## Kontext

Der Zahlungspartner (Empfänger/Auftraggeber) einer Buchung existierte nur als zwei unnormalisierte String-Spalten auf `transactions` (`counterparty_name`, `counterparty_iban`) — ohne Index, ohne Deduplizierung. Derselbe Händler lag zigfach als leicht unterschiedlicher Freitext vor („REWE SAGT DANKE 4711", „REWE Markt GmbH", …). Geplante Features (Regel-Engine, Auswertungen pro Händler, Bulk-Aktionen, Eigenübertrag-Erkennung) brauchen einen stabilen, deduplizierte Partner-Bezug; auf Freitext ist das nicht sauber baubar. Der einzige bestehende Lesezugriff (IBAN-Filter) machte mangels Index und wegen `Replace/ToUpper` in der WHERE-Klausel einen Sequential Scan.

## Entscheidungen

1. **Eigenes Entity `PaymentPartner`** (Tabelle `finance.payment_partners`) mit nullable FK `payment_partner_id` auf `transactions`. Die Roh-Strings bleiben als Audit auf der Buchung, werden aber konsistent zu `payment_partner_name`/`payment_partner_iban` umbenannt (Breaking Change fürs Frontend, bewusst akzeptiert — Naming „Counterparty" war unverständlich).
2. **Scoping per `person_id` (Konto-Owner), nicht per Haushalt.** Konto↔Haushalt ist m:n (`account_households`); ein Konto kann in 0..n Haushalte freigegeben sein — Haushalts-Scoping wäre nicht deterministisch. Alle Schreibpfade autorisieren bereits über den Owner. Haushaltsmitglieder sehen `paymentPartnerId` über die Buchungen (wie `categoryId`).
3. **Matching IBAN-first, deterministisch** (identisch in Resolver und Backfill): mit IBAN nur über `(person_id, iban)` (nie an einen Namens-Match hängen — IBAN-Identität ist stärker); ohne IBAN über `(person_id, normalized_name)` gegen alle Partner, ältester gewinnt; ohne beides kein Partner (Revolut). Konsequenz: „REWE"+IBAN und „REWE" ohne IBAN können koexistieren; Nutzer bereinigt per Merge.
4. **Rename ändert nur `display_name`.** Der Matching-Key `normalized_name` bleibt stabil, sonst würde der nächste Import den alten Partner neu anlegen (genau der Fehlermodus, den Merges ohnehin haben).
5. **Kein Alias-Table in dieser Iteration.** Nach einem Merge geht der Matching-Key des Source verloren; ein künftiger Import mit dessen Namen legt den Partner neu an. Die IBAN-Vererbung beim Merge (Ziel ohne IBAN erbt IBAN + `linked_account_id`) erhält den wichtigsten Key im häufigsten Fall. `payment_partner_aliases` = Future Work.
6. **Eigenüberträge nur markieren, nicht ausschließen:** `linked_account_id` wird gesetzt, wenn die Partner-IBAN zu einem eigenen Konto gehört. Der E+A-Report bleibt unverändert; ein Ausschluss von Umbuchungen ist durch den Link vorbereitet (Future Work).
7. **Beifang:** `accounts.deactivated_from` („deaktiviert ab", nur bei `is_active = false`) — reine Information, keine Schreibsperre.

## Geprüfte Alternativen

- **Nur Indizes auf die Roh-Spalten** — verworfen: löst Duplikate und Schreibweisen-Streuung nicht, keine Basis für Regeln/Merge/Umbenennen.
- **Haushalts-Scoping wie `Category`** — verworfen: nicht deterministisch bei geteilten/ungeteilten Konten (m:n, 0..n).
- **Fuzzy-Name-Matching** — verworfen: unvorhersehbare Zuordnungen; exaktes normalisiertes Matching + manueller Merge ist nachvollziehbar.
- **Alias-Table sofort** — verworfen: verdoppelt Resolver-Komplexität und Unique-Regeln für einen Randfall; nachrüstbar.

## Konsequenzen

- Import/Create/Update lösen Partner automatisch auf (`PaymentPartnerResolver`, set-basiert: eine Query pro Import-Batch). Parallele Imports können beim Anlegen desselben Partners kollidieren — die partial-unique Indizes (`(person_id, iban) WHERE iban IS NOT NULL`, `(person_id, normalized_name) WHERE iban IS NULL`) lassen das laut fehlschlagen; Retry genügt.
- Migration `paymentPartners`: RenameColumn (verlustfrei) + SQL-Backfill (IBAN-Gruppen, dann Nur-Name-Gruppen; Anzeigename = häufigster bereinigter Rohname; Verlinkung + Eigenkonto-Erkennung). `Down()` verlustfrei, da die Roh-Strings auf der Buchung bleiben. Import-Hashes bleiben stabil (Hash nutzt nur Werte, keine Spaltennamen) — Re-Importe erzeugen keine Duplikate.
- **Locale-Caveat:** SQL `upper()` (Backfill) und .NET `ToUpperInvariant()` (Resolver) können bei Sonderzeichen (z. B. `ß`) abweichen; schlimmstenfalls entsteht ein Duplikat, das per Merge bereinigt wird — akzeptiert.
- Breaking Changes fürs Frontend: Feld-/Parameter-Renames (`counterparty*` → `paymentPartner*`), neues `paymentPartnerId` auf Buchungen, neue Endpoints `GET/PATCH /PaymentPartner`, `POST /PaymentPartner/merge`, Filter `GET /Transaction?paymentPartnerId=`, `deactivatedFrom` auf Account.
