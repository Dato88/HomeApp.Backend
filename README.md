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

Der AppHost setzt die OAuth-Konfiguration automatisch per Environment Variables.

### Angular-Integration (externes Repo)

Kein OAuth-Code im Frontend — nur session-basierte BFF-Kommunikation:

- Login: `window.location.href = 'http://localhost:5555/auth/login'`
- Logout: Redirect zu `http://localhost:5555/auth/logout`
- API-Calls: `http.get('http://localhost:5555/api/...', { withCredentials: true })`
- Auth-Status: `GET http://localhost:5555/auth/status`

## Person-Provisioning (Erst-Login)

Keycloak übernimmt nur die Authentifizierung. Business-Daten liegen in der API-Datenbank (`people`-Tabelle).

**Ablauf nach Login:**

1. Angular ruft z.B. `GET /api/Person/person` auf (via BFF mit Cookie).
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
