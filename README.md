# HomeApp.Backend

Backend der HomeApp: eine REST-API für Haushaltsverwaltung (Todos, Budgets, Personen) mit vorgeschaltetem BFF (Backend for Frontend), der die session-basierte Authentifizierung gegen Keycloak übernimmt. Konsumiert wird die API vom Angular-Frontend (externes Repo).

- **Projekt-Doku-Owner:** Andrej Miller
- **Projektseite (Confluence):** [Link — nachtragen]

## Voraussetzungen

- .NET SDK 10.x (Target Framework `net10.0`)
- Docker (Keycloak- und PostgreSQL-Container werden vom Aspire AppHost gestartet)

**Bekannte Fallstricke:**

- OAuth ist **Pflicht** — die API startet nicht ohne `OAuth:Authority` und `OAuth:ValidAudiences`. Beim Start über den AppHost werden diese automatisch per Environment Variables gesetzt.
- Keycloak importiert den Realm nur beim ersten Start eines frischen Volumes. Nach Änderungen an [`homeapp-realm.json`](./HomeApp.Backend/HomeApp.Backend.AppHost/keycloak/homeapp-realm.json) das Volume zurücksetzen, sonst wird der Import übersprungen:

  ```bash
  docker volume ls | grep keycloakContainer
  docker volume rm <volume-name>
  ```

## Quick Start

```bash
cd HomeApp.Backend
dotnet run --project HomeApp.Backend.AppHost
```

Die Anwendung läuft anschließend auf:

| Dienst | URL |
|--------|-----|
| BFF (Einstiegspunkt) | http://localhost:5555 |
| Web.Api | http://localhost:7254 |
| Aspire Dashboard | URL aus der Konsolenausgabe |

**Test-User (Realm-Import):** `devuser` / `devpassword`

## Infrastruktur

Benötigte Drittsysteme (lauffähige Definition im Repo: [`AppHost.cs`](./HomeApp.Backend/HomeApp.Backend.AppHost/AppHost.cs)):

| System | Zweck | Lokaler Zugriff |
|--------|-------|-----------------|
| Keycloak 26 | Identity Provider (OIDC) | http://localhost:8080 (`admin` / Aspire-Parameter `keycloak-admin-password`) |
| PostgreSQL `homeappContainer` | Persistenz der API | localhost:5060 |
| PostgreSQL `keycloakContainer` | Persistenz Keycloak | dynamischer Port (siehe Aspire Dashboard) |

Start/Stopp: `dotnet run --project HomeApp.Backend.AppHost` / `Ctrl+C` (Container verwaltet Aspire).

**Alternativ (produktionsnah, API hinter nginx):** [`Docker/docker-compose.yml`](./HomeApp.Backend/Docker/docker-compose.yml)

```bash
cd HomeApp.Backend/Docker
cp .env.example .env    # Platzhalter ersetzen
# Selbstsigniertes Dev-Zertifikat für nginx erzeugen (einmalig, git-ignoriert):
openssl req -x509 -newkey rsa:2048 -nodes -days 365 -subj "/CN=localhost" \
  -keyout nginx/cert.key -out nginx/cert.pem
docker compose up -d    # Stopp: docker compose down
```

## Konfiguration

Alle Konfigurationsschlüssel für den Compose-Betrieb: siehe [`Docker/.env.example`](./HomeApp.Backend/Docker/.env.example).
**Keine echten Secrets einchecken — nur Platzhalter und lokale Dummy-Werte.**

Secrets gehören in User Secrets bzw. Aspire-Parameter — nicht in `appsettings.json`:

```jsonc
// User Secrets für Web.Api (nötig für Standalone-Start und EF-Migrations):
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

```jsonc
// User Secrets für HomeApp.Backend.AppHost — überschreibt die Dev-Defaults der Aspire-Parameter:
{
  "Parameters": {
    "keycloak-admin-password": "<wert>",
    "bff-client-secret": "<wert>"
  }
}
```

Das BFF-Client-Secret speist der AppHost aus dem Parameter `bff-client-secret` in **beide** Seiten: als `OAuth__ClientSecret` in den BFF und als `${BFF_CLIENT_SECRET}` in den Keycloak-Realm-Import. Für den BFF-Standalone-Start: `dotnet user-secrets set "OAuth:ClientSecret" "<wert>" --project HomeApp.Bff`.

## Tests & Build

```bash
# identisch zur CI-Pipeline (.github/workflows/dotnet.yml):
dotnet restore ./HomeApp.Backend/HomeApp.Backend.sln
dotnet build ./HomeApp.Backend/HomeApp.Backend.sln --no-restore
dotnet test ./HomeApp.Backend/HomeApp.Backend.sln --no-build
```

## Datenbank & Migrations

Migrations liegen in [`Infrastructure/Migrations`](./HomeApp.Backend/Infrastructure/Migrations/). Befehle aus `HomeApp.Backend/` ausführen; sie setzen die oben beschriebenen Web.Api-User-Secrets voraus (OAuth ist Pflicht beim Host-Start):

```bash
# Migrations ausführen:
dotnet ef database update --project Infrastructure --startup-project Web.Api
# Neue Migration:
dotnet ef migrations add <Name> --project Infrastructure --startup-project Web.Api
# Rollback:
dotnet ef database update <VorherigeMigration> --project Infrastructure --startup-project Web.Api
```

## Weiterführende technische Doku

Interne Architektur (Authentifizierung, BFF-Integration, Person-Provisioning) und repo-spezifische Architecture Decision Records (ADRs): [`docs/`](./docs/)

## Links

- Confluence-Projektseite: [Link — nachtragen]
- CI-Pipeline: [`.github/workflows/dotnet.yml`](./.github/workflows/dotnet.yml) (GitHub Actions)
- Generierte API-Doku: Scalar UI unter http://localhost:7254/scalar (nur Development)
