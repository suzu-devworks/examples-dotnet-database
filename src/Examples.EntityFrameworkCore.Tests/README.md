# Examples.EntityFrameworkCore.Tests

## Table of Contents <!-- omit in toc -->

- [Index](#index)
  - [Examples.EntityFrameworkCore Tests](#examplesentityframeworkcore-tests)
  - [ContosoUniversity Tests](#contosouniversity-tests)
- [Create the database](#create-the-database)
- [Microsoft.EntityFrameworkCore.InMemory](#microsoftentityframeworkcoreinmemory)
- [Microsoft.EntityFrameworkCore.SQLite](#microsoftentityframeworkcoresqlite)
  - [Types of connection strings](#types-of-connection-strings)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Index

### Examples.EntityFrameworkCore Tests

- [Metadata/](./EntityFrameworkCore.Tests/Metadata/)

### ContosoUniversity Tests

- [Tutorials/](./EntityFrameworkCore.SQLite.Tests/ContosoUniversity/Tutorials/)
- [Repositories/](./EntityFrameworkCore.SQLite.Tests/ContosoUniversity/Repositories/)

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

## Microsoft.EntityFrameworkCore.InMemory

I tried using `Microsoft.EntityFrameworkCore.InMemory` partway through, but the lack of transactions was a fatal problem, making it impossible to write test code.

## Microsoft.EntityFrameworkCore.SQLite

### Types of connection strings

A standard connection string, persisted in the specified file.

`Data Source=Application.db;Cache=Shared`

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

## Examples.EntityFrameworkCore
dotnet new classlib -o src/Examples.EntityFrameworkCore
dotnet sln add src/Examples.EntityFrameworkCore
cd src/Examples.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Relational
cd ../../

## Examples.EntityFrameworkCore.Tests
dotnet new xunit3 -o src/Examples.EntityFrameworkCore.Tests
dotnet sln add src/Examples.EntityFrameworkCore.Tests
cd src/Examples.EntityFrameworkCore.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Configuration.Binder

dotnet add reference ../Examples.EntityFrameworkCore/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated

dotnet new tool-manifest
dotnet tool install dotnet-ef
```
