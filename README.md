# HomeApp.Backend

## Lokale Entwicklung

### Ohne Keycloak (Dev-Stub)

Standardmäßig läuft die API ohne OAuth-Konfiguration mit einem festen `DevUserContext` aus `appsettings.Development.json`:

```json
"DevUserContext": {
  "PersonId": 1,
  "UserId": "00000000-0000-0000-0000-000000000001",
  "Email": "dev@homeapp.local"
}
```

Die `PersonId` muss auf eine existierende Person in der Datenbank zeigen.

### Mit Keycloak + BFF (Aspire)

```bash
cd HomeApp.Backend
dotnet run --project HomeApp.Backend.AppHost
```

| Dienst | URL |
|--------|-----|
| Keycloak Admin | http://localhost:8080 (admin / Parameter `keycloak-admin-password`) |
| BFF | http://localhost:5000 |
| API | dynamischer Port via Aspire Dashboard |

**Test-User (Realm-Import):** `devuser` / `devpassword`

**BFF Client-Secret:** Aspire-Parameter `bff-client-secret` (Dev: `local-homeapp-bff-secret` aus Realm-Export)

### Angular-Integration (externes Repo)

Kein OAuth-Code im Frontend — nur session-basierte BFF-Kommunikation:

- Login: `window.location.href = 'http://localhost:5000/auth/login'`
- Logout: Redirect zu `http://localhost:5000/auth/logout`
- API-Calls: `http.get('http://localhost:5000/api/...', { withCredentials: true })`
- Auth-Status: `GET http://localhost:5000/auth/status`

## Konfiguration

### OAuth (Resource Server)

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
  }
}
```
