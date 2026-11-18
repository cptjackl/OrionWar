# OrionWar

A web-based space strategy game inspired by Neptune's Pride 2. This repository currently contains the backend API; the frontend (`orionwar-web`) will be added later.

## Projects

| Project | Purpose |
|---|---|
| `OrionWar.Api` | ASP.NET Core API: endpoints, configuration, (later) EF Core, auth, SignalR, tick service |
| `OrionWar.Game` | Pure game rules and simulation library, no web or database dependencies |
| `OrionWar.Tests` | xUnit tests |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://docs.docker.com/get-docker/) (for PostgreSQL)

## Getting started

1. Create your local env file and set the values:
   ```bash
   cp .env.example .env
   ```
2. Start PostgreSQL:
   ```bash
   docker compose up -d
   ```
3. Store the connection string in user secrets. Use the same values and port as in `.env`:
   ```bash
   dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=<POSTGRES_PORT>;Database=<POSTGRES_DB>;Username=<POSTGRES_USER>;Password=<POSTGRES_PASSWORD>" --project OrionWar.Api
   ```
4. Run the API:
   ```bash
   dotnet run --project OrionWar.Api
   ```
5. Check it is up: <http://localhost:5082/health>. The OpenAPI document is served in Development.

## Configuration

- `appsettings.json` holds non-secret defaults, such as `Game:TickIntervalSeconds`.
- Secrets (connection string, later the JWT key) live in `.env` (Docker) and user secrets (API). Never commit them.

## Build and test

```bash
dotnet build
dotnet test
```
