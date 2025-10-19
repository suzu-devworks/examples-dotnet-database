# Examples.EntityFrameworkCore.SQLite.Tests

## Table of Contents <!-- omit in toc -->

- [Create the database](#create-the-database)
- [Types of connection strings](#types-of-connection-strings)
  - [Basic](#basic)
  - [Sharable in-memory](#sharable-in-memory)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
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

## Types of connection strings

### Basic

A standard connection string, persisted in the specified file.

`Data Source=Application.db;Cache=Shared`

### Sharable in-memory

SQLite uses the in-memory format below.

`Data Source=:memory:`

However, this format doesn't work properly with EntityFramework tests because Open and Close are performed automatically.

Therefore, we'll use a shareable in-memory format.

`Data Source=Sharable:Mode=Memory;Cache=Shared`

- [Connection strings - Microsoft.Data.Sqlite | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/standard/data/sqlite/connection-strings)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.EntityFrameworkCore.SQLite.Tests
dotnet new xunit -o src/Examples.EntityFrameworkCore.SQLite.Tests
dotnet sln add src/Examples.EntityFrameworkCore.SQLite.Tests
cd src/Examples.EntityFrameworkCore.SQLite.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add reference src/Examples.EntityFrameworkCore/
dotnet add reference src/Examples.Various/
dotnet add reference src/Examples.ContosoUniversity/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated

dotnet new tool-manifest
dotnet tool install dotnet-ef
```
