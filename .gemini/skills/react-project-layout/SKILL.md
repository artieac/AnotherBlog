# TechnologyRadarUI — Project Layout

This document describes the structural conventions for this codebase. Use it as a reference when deciding where to place new code or find existing code.

## Top-Level Source Root

All React source lives under `src/react/`.

---

## Repositories (`src/react/Repositories/`)

**All API integrations are done through repository classes.** Every class that communicates with a backend API must live here. Each file wraps a specific domain (Radar, RadarTemplate, Team, User, Role, etc.) and exports functions that call the REST API via `RestClient.ts`.

- Never call APIs directly from components, pages, or Redux — always go through a Repository.
- Current repositories: `RadarRepository`, `RadarItemRepository`, `RadarSubjectRepository`, `RadarTemplateRepository`, `TeamRepository`, `UserRepository`, `RoleRepository`, `RestClient`.

---

## Apps (`src/react/Apps/`)

Each application is self-contained in its own folder. There are currently four:

| Folder | Purpose |
|---|---|
| `MainSiteApp/` | The public-facing site for viewing technology radars |
| `ManageRadarsApp/` | Authenticated user area for managing radar data |
| `AdminApp/` | System administration (manage users, system-level config) |
| `Common/` | Shared code used across two or more of the above apps (e.g. NavBar, FooterComponent, UserPage, ProviderStore, ConfigurationSettings, Utilities) |

### Structure within each App

```
Apps/<AppName>/
  Pages/
    <PageName>/          ← One folder per page (a single cohesive view)
      index.js           ← The page component
      <UniqueComponent>/ ← Components used only by this page live here
        <SubComponent>/  ← Components unique to a component nest under it
  NavBarRowDefinition/   ← App-specific nav bar config
```

- **Pages** represent a single cohesive view. A page composes components to form that view.
- **Components unique to a page** nest inside that page's folder.
- **Components unique to a component** nest inside that component's folder.
- Do not put a page-specific component in the global `components/` folder.

---

## Generic UI Components (`src/react/components/`)

Completely generic, reusable display primitives used across any app or page. Examples: grids, tables, dropdowns, loading indicators, error boundaries, list components.

- These components have no knowledge of any specific domain or app.
- Current components: `DivTableComponent`, `AppendableTableComponent`, `DropdownComponent`, `LoadingComponent`, `MessageComponent`, `NavBarComponent`, `ErrorBoundaryComponent`, `AsyncDataComponent`, etc.
- Add new components here only if they are truly generic and reusable across multiple apps.

---

## Redux (`src/react/redux/`)

**All Redux-related code goes here.** This includes reducers, actions, action creators, and selectors for global state.

- Current reducers: `RadarReducer`, `RadarTemplateReducer`, `TeamReducer`, `UserReducer`, `ErrorReducer`.
- Exception: `MainSiteApp` has a local `redux/` folder for app-specific state (`RadarItemReducer`) that does not belong in global state.
- When adding new state, determine first whether it is app-local or global before deciding which `redux/` folder to use.

---

## Models (`src/react/Models/`)

TypeScript interfaces and types for shared data shapes (e.g. `IRadarTemplateDetail`). Place new interfaces here when they are shared across multiple files or layers.

---

## Decision Guide

| I need to... | Put it in... |
|---|---|
| Call a backend API | `Repositories/` |
| Add a new page to an app | `Apps/<AppName>/Pages/<NewPage>/` |
| Add a component only one page uses | `Apps/<AppName>/Pages/<PageName>/<NewComponent>/` |
| Add a component only one component uses | `Apps/<AppName>/Pages/<PageName>/<ParentComponent>/<NewComponent>/` |
| Add a component shared across multiple apps | `Apps/Common/` |
| Add a generic display/UI primitive | `components/` |
| Add global Redux state (actions + reducer) | `redux/` |
| Add a TypeScript interface/type | `Models/` |
