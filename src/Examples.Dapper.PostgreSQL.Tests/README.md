# Examples.Dapper.PostgreSQL.Tests

## Table of Contents <!-- omit in toc -->

- [Dapper Overview](#dapper-overview)
- [Connection String](#connection-string)
- [Dapper Bulk Insert](#dapper-bulk-insert)
- [Dapper Type Handler](#dapper-type-handler)
- [SqlBuilder](#sqlbuilder)
  - [`Dapper.SqlBuilder`](#dappersqlbuilder)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)
- [References](#references)

## Dapper Overview

Dapper is a simple micro-ORM used to simplify development using ADO.NET.

What it does is simplify various aspects of ADO.NET by providing extension methods to `DbConnection`.

## Connection String

- [Connection String Parameters | Npgsql Documentation](https://www.npgsql.org/doc/connection-string-parameters.html)

## Dapper Bulk Insert

Dapper allows you to pass a list as a parameter, but it does not perform a bulk insert. Instead, it iterates through the list of items and executes the Execute method for each item using a single insert statement.

When inserting a large amount of data, it is common practice to consider issuing a single SQL statement that inserts multiple rows of data at once.

```cs
  var sql = $$"""
      INSERT INTO products (category_id, name, description, unit_price)
      VALUES
          {{string.Join(",", products.Select((_, i) => $"(@CategoryID_{i}, @Name_{i}, @Description_{i}, @UnitPrice_{i})"))}}
      """;
```

SQL itself is fine, but the problem lies with the parameter data.

This is where Dapper's `DynamicParameters` type or `ExpandoObject` comes in handy.

## Dapper Type Handler

Data type conversion is performed using `SqlMapper.TypeHandler<T>`, and this can be done by registering it globally.

```cs
  SqlMapper.AddTypeHandler(new UniversalDateTimeTypeHandler());
```

It would be convenient if we could specify settings on a session-by-session basis or a column-by-column basis...

## SqlBuilder

### `Dapper.SqlBuilder`

There aren't many lines of code, so it would be best to read the code itself.

It's just that simple.

- [Dapper/Dapper.SqlBuilder at main · DapperLib/Dapper](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Dapper.PostgreSQL.Tests
dotnet new xunit3 -o src/Examples.Dapper.PostgreSQL.Tests
dotnet sln add src/Examples.Dapper.PostgreSQL.Tests
cd src/Examples.Dapper.PostgreSQL.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
dotnet add package Dapper
dotnet add package Npgsql
dotnet add package Dapper.SqlBuilder

dotnet add reference ../Examples.Dapper/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```

## References

- [Welcome To Learn Dapper ORM - A Dapper Tutorial for C# and .NET Core](https://www.learndapper.com/dapper-query/selecting-scalar-values)
- [DapperLib/Dapper: Dapper - a simple object mapper for .Net](https://github.com/DapperLib/Dapper)
