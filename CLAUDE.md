# CLAUDE.md

## Project Overview

.NET Cameroon Community Website — official site for the .NET Cameroon community at [dotnet.cm](https://dotnet.cm).

## Architecture

- **Blazor SSR** for public pages (SEO-friendly)
- **React SPA** (`src/admin/`) for admin dashboard (migrating from Blazor WASM)
- **Clean layered architecture**: app → app.infrastructure → app.business → app.domain → app.shared
- **Dual database**: SQL Server (primary data), SQLite (projects cache)
- **EF Core 10** with domain events interceptor, complex properties for value objects

## Tech Stack

- .NET 10, ASP.NET Core, Blazor SSR
- React + Vite + TanStack Router/Query (admin)
- Tailwind CSS 4
- SQL Server 2022, SQLite
- Docker Compose for local dev
- OpenTelemetry + Aspire Dashboard

## Setup

```bash
# Start infrastructure
docker compose up -d

# Install JS dependencies
pnpm install

# Build Tailwind CSS
pnpm run tailwind

# Run the app
dotnet run --project ./src/app

# Dev mode (Tailwind + dotnet)
pnpm run dev
```

## Key Commands

```bash
# Run tests
dotnet test src

# Build Tailwind CSS
pnpm run tailwind

# Watch Tailwind
pnpm run tailwind-watch

# Admin dashboard dev server
pnpm run admin:dev

# Build admin dashboard
pnpm run admin:build
```

## Package Management

- Use **pnpm** (not npm) for all JS/TS package management

## Project Structure

```
src/
  app/                    # Main ASP.NET Core web app (Blazor SSR)
  app.domain/             # Domain entities, aggregates, value objects
  app.business/           # Service interfaces, repository contracts
  app.infrastructure/     # EF Core, service implementations
  app.client/             # Blazor WASM (being removed — migrating to React)
  app.shared/             # Shared utilities, constants
  admin/                  # React admin dashboard (Vite + TanStack)
  dotnet-ef-seeder/       # Custom DB seeding package
```

## Conventions

- Minimal API pattern for endpoints (see `src/app/Api/`)
- ErrorOr result pattern for service layer operations
- Domain-Driven Design: aggregates, value objects, domain events
- Localization: en-US, fr-FR via RESX files
- Auth: Cookie auth (UI/admin), JWT Bearer (external API)
- Feature branch workflow: `feature/<name>` from `main`
