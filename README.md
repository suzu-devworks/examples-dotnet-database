# examples-dotnet-database

![Dynamic XML Badge](https://img.shields.io/badge/dynamic/xml?url=https%3A%2F%2Fraw.githubusercontent.com%2Fsuzu-devworks%2Fexamples-dotnet-database%2Frefs%2Fheads%2Fmain%2Fsrc%2FDirectory.Build.props&query=%2F%2FLTSFrameworks&logo=dotnet&label=Frameworks)
[![build](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/dotnet-build.yml)
[![CodeQL](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-database/actions/workflows/github-code-scanning/codeql)

## What is the purpose of this repository?

This repository serves as my personal sandbox for learning and experimenting with database programming using .NET and C#.

The content here may be useful for other developers who are facing similar challenges.

However, please note that the code presented here is based solely on my personal opinions and may contain errors or inaccuracies.

## Create `db_password.txt`

First, make sure you have the password for your database instance ready.

For example:

```shell
cat /dev/urandom | tr -dc 'a-zA-Z0-9!@#\$%&/:;\^()_+\-=<>?' | fold -w 24 | head -n 1 > .password.txt
```

## Multiple database environments

This repository uses multiple DevContainers configurations to switch between different databases.

As of now, it is as follows:

- [*default*](./.devcontainer/): EntityFrameworkCore.InMemory database and SQLite
- [`mssql`](./.devcontainer/mssql/): SQL server 2022 express edition
- [`pgsql`](./.devcontainer/pgsql/): PostgreSQL 18

To switch between containers, please use the `Dev Containers: Switch Container` command from the Command Palette (F1).
