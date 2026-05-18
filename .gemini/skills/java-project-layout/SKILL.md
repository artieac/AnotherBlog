---
name: java-project-layout
description: Use this skill whenever working in this Java codebase — when adding new files, deciding where code belongs, reviewing placement of classes, or discussing architecture. Covers the layered architecture with data, domainmodel, services, and web layers. Trigger on questions like "where should I put this", "how is the code organized", "what layer does X belong in", or any time new Java classes need to be created.
version: 1.0.0
---

# Java Project Layout

This project is a Spring Boot application. All production code lives under:

```
src/main/java/com/alwaysmoveforward/technologyradar/
```

The code is organized into four primary layers: `data`, `domainmodel`, `services`, and `web`. Each layer has a strict role and dependencies only flow inward (web → services → domainmodel/data, never upward).

---

## Layer: `data/`

The data layer owns everything related to external systems — databases, third-party APIs, etc. Nothing outside this layer should know that these external systems exist.

### `data/Entities/`

JPA entity classes that mirror the database schema (or an external API contract). These are pure data transfer objects — they carry no business logic.

**Rule:** Entities are private to the data layer. No class outside `data/` should ever import or reference an Entity.

**Existing entities:** `RadarEntity`, `RadarItemEntity`, `RadarCategoryEntity`, `RadarRingEntity`, `RadarTemplateEntity`, `AssociatedRadarTemplateEntity`, `RadarUserEntity`, `TeamEntity`, `TechnologyEntity`, `UserTypeEntity`, `VersionedIdEntity`

### `data/repositories/`

Repository classes that perform all I/O against external systems. Repositories translate between the external world (Entities or raw API responses) and the domain (Domain Model objects or primitives).

**Rules:**
- Method parameters must be primitive/base types or Domain Model objects — never Entities.
- Return values must be primitive/base types or Domain Model objects — never Entities.
- Repositories use the `data/mapper/` classes internally to convert between Entities and Domain Models before returning results.
- External API integrations (e.g., `Auth0Repository`) live here alongside database repositories.

**Existing repositories:** `RepositoryBase`, `RadarRepositoryBase`, `FullRadarRepository`, `PublicRadarRepository`, `RadarCategoryRepository`, `RadarRingRepository`, `RadarTemplateRepository`, `RadarUserRepository`, `TeamRepository`, `TechnologyRepository`, `UserTypeRepository`, `Auth0Repository`, `SimpleDomainRepository`

### `data/mapper/`

Mapper classes that define how to convert between Entities and Domain Models. Used internally by repositories — callers of repositories never invoke mappers directly.

**Existing mappers:** `RadarMapper`

### `data/dao/`

Spring Data JPA DAO interfaces (extending `JpaRepository` or similar) used by the repository implementations to execute queries.

**Existing DAOs:** `RadarDAO`, `RadarItemDAO`, `RadarCategoryDAO`, `RadarRingDAO`, `RadarTemplateDAO`, `AssociatedRadarTemplateDAO`, `RadarUserDAO`, `TeamDAO`, `TechnologyDAO`, `UserTypeDAO`

---

## Layer: `domainmodel/`

Domain Model objects are the core business objects of the system, following Domain-Driven Design principles.

**Rules:**
- Business logic that belongs to a single entity belongs here as methods on the Domain Model class.
- Domain Models know nothing about persistence, HTTP, or external systems.
- Prefer putting business rules on Domain Models over putting them in services.

**Existing domain models:** `Radar`, `RadarCategory`, `RadarItem`, `RadarRing`, `RadarTemplate`, `RadarUser`, `Team`, `Technology`, `Auth0UserProfile`, `Permissions`, `Role`, `UserRights`, `UserType`, `MostRecentRadarComparator`

---

## Layer: `services/`

Domain Services coordinate work across multiple repositories or aggregate roots, and implement business rules that don't cleanly belong to a single Domain Model.

**Rules:**
- Services call repositories — they never interact with Entities directly.
- Services operate on Domain Models, not Entities.
- Cross-cutting business logic that requires combining data from multiple repositories belongs here.
- Sub-packages (`RadarInstance/`, `RadarTemplate/`) group related services by bounded context.

**Existing services:** `ServiceBase`, `RadarUserService`, `TeamService`, `TechnologyService`, `DiagramConfigurationService`, `RadarService` (under `RadarInstance/`), `RadarAccessManager`, `RadarRepositoryFactory`, `AssociatedRadarTemplateService`, `DefaultRadarTemplateManager`, `RadarTemplateService` (under `RadarTemplate/`)

---

## Layer: `web/`

The web layer is the outermost layer — it handles HTTP concerns only. It calls services (never repositories directly) and translates between HTTP request/response formats and Domain Models.

### `web/API/`

Spring MVC `@RestController` classes that define all REST endpoints exposed by this application. Controllers should be thin — delegate business logic to services, not implement it.

**Existing controllers:** `RadarController`, `RadarItemController`, `RadarConfigurationController`, `RadarSubjectController`, `RadarTemplateController`, `RoleController`, `TeamController`, `UserController`, `HomeController`, `LoginController`, `CallbackController`, `ControllerBase`

### `web/Models/`

View Model classes — the response shapes returned by the API controllers. These represent the API contract with clients and may aggregate or reshape Domain Model data for presentation.

**Existing view models:** `RadarViewModel`, `RadarTemplateViewModel`, `RadarTemplateDetailMessage`, `RadarSubjectBreakdown`, `RadarSubjectBreakdownItem`, `DiagramPresentation`, `RadarRingPresentation`, `RadarDrawingSurface`, `Quadrant`, `QuadrantItem`, `PolarCoords`, `TeamViewModel`, `UserViewModel`, `PublishRadarModel`

---

## Layer: `security/`

Cross-cutting authentication and authorization infrastructure. Auth0 integration, JWT management, and Spring Security configuration live here.

Sub-packages: `Auth0/` (Auth0 integration), `jwt/` (JWT filter/manager classes)

---

## Decision Guide: Where Does New Code Go?

| What you're building | Where it goes |
|---|---|
| JPA entity / API response DTO | `data/Entities/` |
| Spring Data JPA interface | `data/dao/` |
| Entity ↔ Domain Model conversion | `data/mapper/` |
| DB query / external API call | `data/repositories/` |
| Core business object | `domainmodel/` |
| Business rule on one object | Method on the Domain Model class |
| Business rule across multiple objects/repos | `services/` |
| REST endpoint (`@RestController`) | `web/API/` |
| API response shape / view model | `web/Models/` |

---

## Dependency Rules (strictly enforced)

```
web/API  →  services  →  domainmodel
                     →  data/repositories  →  data/mapper  →  data/Entities
                                          →  data/dao
```

- `web` may import `services` and `domainmodel` — never `data`
- `services` may import `domainmodel` and `data/repositories` — never `data/Entities`
- `data/repositories` is the only code that may import `data/Entities`
- No upward dependencies allowed
