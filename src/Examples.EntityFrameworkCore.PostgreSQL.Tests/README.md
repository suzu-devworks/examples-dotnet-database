# Examples.EntityFrameworkCore.PostgreSQL.Tests

## Table of Contents <!-- omit in toc -->

- [Create the database](#create-the-database)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
dotnet tool install --global dotnet-ef

# or restore workspace tools
dotnet tool restore
```

Add a Migration:

```shell
dotnet ef migrations add InitialCreate
```

Apply Migrations:

```shell
dotnet ef database update
```

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.EntityFrameworkCore
## Examples.ContosoUniversity
## Examples.Xunit

## Examples.EntityFrameworkCore.PostgreSQL.Tests
dotnet new xunit -o src/Examples.EntityFrameworkCore.PostgreSQL.Tests
dotnet sln add src/Examples.EntityFrameworkCore.PostgreSQL.Tests
cd src/Examples.EntityFrameworkCore.PostgreSQL.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

dotnet add reference src/Examples.EntityFrameworkCore/
dotnet add reference src/Examples.Xunit/
dotnet add reference src/Examples.ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated

dotnet new tool-manifest
dotnet tool install dotnet-ef
```
