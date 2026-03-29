# GitHub Copilot Instructions

## Language Policy

- **Think and reason in English.**
- **Write all code, comments, and documentation in English.**
- **Respond to the user in Japanese in the chat.**

---

## Repository Purpose and Style

- This repository is a personal learning and experimentation workspace for .NET database technologies.
- Prefer clarity, reproducibility, and educational value over production-level abstractions.

## Test Style and Naming

- Test code is written in unit-test form, but the primary goal is learning code patterns.
- Prefer BDD-style names using `When_..._Then_...` (or equivalent) over AAA-style naming.
- Keep method names descriptive and scenario-focused.

Examples:

- `FindAsync_WhenPrimaryKeyIsProvided_ReturnsSpecifiedRecord`
- `When_ExecutedFromNonTargetEnvironment_Then_StillSucceeds`

## Database and Container-Aware Tests

- Some tests are intentionally not executed unless the target database/container is available.
- Do not remove or rewrite intentional skip logic unless explicitly requested.
- Keep environment-gated tests aligned with the repository pattern:
  - SQL Server tests: gate by `MSSQL_SERVICE`
  - PostgreSQL tests: gate by `POSTGRES_SERVICE`
  - Use environment checks such as `DatabaseEnvironment.IsAvailable` with xUnit skip conditions.

## Dev Container Context

- The repository uses multiple dev container targets:
  - default: in-memory and SQLite-oriented work
  - `mssql`: SQL Server-oriented work
  - `pgsql`: PostgreSQL-oriented work
- When adding or modifying DB-dependent code/tests, preserve behavior in non-target containers.

## Change Safety

- Avoid broad refactors when a focused, small change is sufficient for the learning objective.
- Keep existing project conventions, structure, and examples consistent across test projects.
