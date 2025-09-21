# examples-dotnet-database

[![build](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/dotnet-build.yml)
[![CodeQL](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/github-code-scanning/codeql)

## What is the purpose of this repository?

This repository serves as my personal sandbox for learning and experimenting with database programming using .NET and C#.

The content here may be useful for other developers who are facing similar challenges.

However, please note that the code presented here is based solely on my personal opinions and may contain errors or inaccuracies.

## Multiple database environments

This repository uses multiple DevContainers configurations to switch between different databases.

As of now, it is as follows:

- [default](./.devcontainer/): All databases for container creation
- [`dev-only`](./.devcontainer/dev-only/): EntityFrameworkCore.InMemory database or SQLite
- [`mssql`](./.devcontainer/mssql/): SQL server 2022 express edition
- [`pgsql`](./.devcontainer/pgsql/): PostgreSQL 17

To switch between containers, please use the `Dev Containers: Switch Container` command from the Command Palette (F1).
