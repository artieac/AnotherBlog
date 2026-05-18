# AnotherBlog Project Instructions

This document codifies the architectural principles, coding standards, and workflows for the AnotherBlog project. These instructions are foundational and take precedence over general defaults.

## Frontend Architecture (React)

- **Layout & Structure:** The Admin React tool must strictly follow the file structure and conventions defined in the `react-project-layout` skill.
- **Styling:** Maintain consistency between the WYSIWYG editor (Quill) and public-facing views. Use the `ql-snow` and `ql-editor` classes for rendering content to ensure layout integrity (e.g., bullet points, indentation).
- **Cache Busting:** All static assets in the Admin tool should utilize ASP.NET Core's `asp-append-version="true"` to ensure immediate delivery of updates after a build.

## Backend Architecture (.NET)

- **Domain-Driven Design (DDD):** Adhere to DDD principles as defined in the `domain-driven-design-review` skill.
- **Data Layer Priority:** **Entity Framework Core** (`src/DataLayer.EntityFramework`) is the primary and only fully implemented data access layer. Other data layer projects (NHibernate, LINQ, MOQ) should be treated as legacy or secondary and only modified if explicitly requested.
- **Authentication & Identity:** User identity is managed via `SecurityPrincipal` and the `CookieAuthenticationFilter`. When performing operations requiring an author or user context, always resolve the `AnotherBlogUser` through the established security context or current session.
- **Project Structure:**
    - **Common Project:** Contains the **Domain Models**. Business logic and rules should be encapsulated within the Domain Models as much as possible to avoid an **Anemic Domain Model**.
    - **Data Project:** Responsible for the data access layer, specifically the mappings required to translate database entities to the Domain Model.
    - **Repositories:** Act as the exclusive interface for data storage. Database-specific logic or storage details must **not** leak out of the repository layer into the Business or Web layers.
- **Services:** Coordinate between repositories and domain models, maintaining the boundaries of the domain while handling application-specific workflows.

## Current Feature States

- **Comments:** The comments section (display and submission) is **intentionally hidden** on the public site via the `hidden` class in `Post.cshtml`. Do not re-enable this feature without explicit instructions.

## Workflow & Conventions

- **Database Updates:** SQL scripts are located in the `Database/AnotherBlog` directory.
- **Testing:** Always look for corresponding tests in the `UnitTest` project when modifying business logic or services.
- **Deployment/Build:** The `rebuild.bat` script in the root directory manages the full Docker-based build and restart process.
