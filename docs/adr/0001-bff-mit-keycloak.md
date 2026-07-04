# ADR 0001: Session-basiertes BFF mit Keycloak als Identity Provider

**Status:** akzeptiert · **Datum:** 2026-07

## Kontext

Das Angular-Frontend (externes Repo) braucht authentifizierten Zugriff auf die Web.Api. Zuvor stellte die API selbst signierte JWTs aus (eigene `JWTSettings`, eigene Identity-Datenbank `homeapp_user_identity`, E-Mail-Versand für Registrierung/Passwort-Reset) — Benutzerverwaltung, Token-Ausstellung und Passwort-Sicherheit lagen damit vollständig in eigener Verantwortung. Tokens lagen im Browser und mussten dort gegen XSS geschützt werden.

## Geprüfte Alternativen

- **Eigene JWT-Ausstellung beibehalten** — verworfen: Benutzerverwaltung, Passwort-Hashing, Token-Rotation und E-Mail-Flows selbst zu betreiben ist sicherheitskritischer Eigenbau ohne fachlichen Mehrwert.
- **OIDC direkt im Angular-Frontend (Authorization Code + PKCE, Tokens im Browser)** — verworfen: Access-/Refresh-Tokens im Browser sind per XSS exfiltrierbar; OAuth-Bibliothek und Token-Handling müssten im Frontend gepflegt werden.
- **BFF-Pattern mit serverseitiger Session** — gewählt.

## Entscheidung

Keycloak übernimmt Authentifizierung und Benutzerverwaltung (Realm `homeapp`, Import aus `HomeApp.Backend.AppHost/keycloak/homeapp-realm.json`). Ein eigener BFF (`HomeApp.Bff`, YARP-Reverse-Proxy) führt den OAuth-Flow serverseitig aus und hält alle Tokens ausschließlich in der Server-Session; der Browser erhält nur Session- und Antiforgery-Cookie. Die Web.Api bleibt reiner Resource Server (JWT-Validierung gegen Keycloak). Business-Daten zur Person bleiben in der API-Datenbank; Keycloak speichert keine `PersonId` (siehe [authentication.md](../authentication.md), Person-Provisioning).

## Konsequenzen

- Keine Tokens im Browser — XSS kann keine Tokens exfiltrieren; dafür ist CSRF-Schutz nötig (`X-XSRF-TOKEN`-Header für mutierende Requests).
- Keycloak und dessen PostgreSQL sind zusätzliche lokale Infrastruktur (startet der AppHost automatisch); Realm-Änderungen erfordern einen Volume-Reset (siehe README, Fallstricke).
- Die BFF-Session liegt im `DistributedMemoryCache` — bei mehreren BFF-Instanzen in Produktion ist ein shared Session-Store (z. B. Redis) erforderlich.
- Die alte Identity-Datenbank (`homeapp_user_identity`), `JWTSettings` und der E-Mail-Versand entfallen ersatzlos.
