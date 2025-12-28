# Examples.FluentMigrator.PostgreSQL

## Table of Contents <!-- omit in toc -->

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.FluentMigrator.PostgreSQL
dotnet new classlib -o src/Examples.FluentMigrator.PostgreSQL
dotnet sln add src/Examples.FluentMigrator.PostgreSQL
cd src/Examples.FluentMigrator.PostgreSQL
dotnet add package FluentMigrator
dotnet add package FluentMigrator.Runner
dotnet add package FluentMigrator.Runner.Postgres
dotnet add package Npgsql;
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.CommandLine
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package ConsoleAppFramework
cd ../../
