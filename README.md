# HomeApp.Backend

## Lokale Entwicklung

OAuth ist **Pflicht** — die API startet nicht ohne `OAuth:Authority` und `OAuth:ValidAudiences`. PersonId kommt immer aus der Datenbank via JWT (`sub` → `Person.UserId`).

### Mit Keycloak + BFF (Aspire)

```bash
cd HomeApp.Backend
dotnet run --project HomeApp.Backend.AppHost
```

| Dienst | URL |
|--------|-----|
| Keycloak Admin | http://localhost:8080 (admin / Parameter `keycloak-admin-password`) |
| BFF | http://localhost:5555 |
| API | Port 7254 (Aspire) |

**Test-User (Realm-Import):** `devuser` / `devpassword`

**BFF Client-Secret:** Aspire-Parameter `bff-client-secret`

**Keycloak-Volume zurücksetzen** (einmalig nach Realm-Import-Änderungen, sonst wird der Import übersprungen):

```bash
docker volume ls | grep keycloakContainer
docker volume rm <volume-name>
```

Der AppHost setzt die OAuth-Konfiguration automatisch per Environment Variables.

### Angular-Integration (externes Repo)

Kein OAuth-Code im Frontend — nur session-basierte BFF-Kommunikation über den Angular-Dev-Proxy (`localhost:4200` → BFF `:5555`):

- Login: `GET http://localhost:4200/auth/login` (Proxy → BFF → Keycloak)
- Logout: `GET http://localhost:4200/auth/logout`
- API-Calls: relative Pfade `/api/...` mit `withCredentials: true` (Proxy → BFF → Web.Api)
- Auth-Status: `GET http://localhost:4200/auth/status`
- CSRF-Token: `GET http://localhost:4200/auth/antiforgery` (vor mutierenden Requests)

OAuth-Callback-URI (RedirectUri): `http://localhost:4200/auth/callback`

**Session-only:** Der Browser erhält nur `.AspNetCore.Session` (plus Antiforgery-Cookie). Access-, Refresh- und ID-Tokens liegen ausschließlich serverseitig in der BFF-Session. Keine `access_token`/`refresh_token`/`id_token`-Cookies mehr.

**CSRF-Schutz:** Alle `POST`/`PUT`/`PATCH`/`DELETE`-Requests an `/api/*` benötigen den Header `X-XSRF-TOKEN`. Token holen via `GET /auth/antiforgery` (Response: `{ "token": "..." }`). Mit Angular `HttpClientXsrfModule` funktioniert das automatisch, wenn Cookie- und Header-Name zur BFF-Konfiguration passen (`X-XSRF-TOKEN`).

**Produktion:** Bei mehreren BFF-Instanzen einen shared Session-Store (z. B. Redis) statt `DistributedMemoryCache` verwenden.

## Person-Provisioning (Erst-Login)

Keycloak übernimmt nur die Authentifizierung. Business-Daten liegen in der API-Datenbank (`people`-Tabelle).

**Ablauf nach Login:**

1. Angular ruft z.B. `GET /api/Person/person` auf (via BFF mit Session-Cookie).
2. Die API validiert den Access-Token (JWT).
3. `PersonProvisioningMiddleware` mappt `sub` (Keycloak-UUID) → `Person.UserId`.
4. Existiert keine Person: automatische Anlage aus Token-Claims (`email`, `given_name`, `family_name`, `preferred_username`).
5. `PersonId` wird 8 Stunden im In-Memory-Cache gehalten (`sub → PersonId`).
6. `IExecutionContextAccessor.PersonId` steht für alle weiteren Requests bereit.

**Wichtig:** `PersonId` wird **nicht** in Keycloak gespeichert oder als Token-Claim übertragen.

### Keycloak Protocol Mapper (Access-Token)

Die API liest den **Access-Token**, nicht den ID-Token. Profil-Claims müssen im Access-Token landen:

| Claim | Quelle |
|-------|--------|
| `sub` | Keycloak User-ID (UUID) — nicht überschreiben |
| `email` | User Email |
| `given_name` | First Name |
| `family_name` | Last Name |
| `preferred_username` | Username |

Client-Scopes `profile` + `email` werden vom BFF bereits angefordert. Zusätzlich braucht der API-Client (`local-homeapp-api`) einen Audience-Mapper und ggf. Dedicated Client Scopes mit „Add to access token“.

## Konfiguration

### OAuth (Resource Server, Pflicht)

```json
"OAuth": {
  "Authority": "http://localhost:8080/realms/homeapp",
  "ValidAudiences": ["local-homeapp-api"]
}
```

Client-Secrets und Passwörter gehören in User Secrets / Aspire Parameters — nicht in `appsettings.json`.

## User Secrets (Beispiel)

```json
{
  "ConnectionStrings": {
    "HomeAppConnection": "yourHomeAppConnectionString"
  },
  "OAuth": {
    "Authority": "http://localhost:8080/realms/homeapp",
    "ValidAudiences": ["local-homeapp-api"]
  }
}
```
