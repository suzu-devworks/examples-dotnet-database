# Examples.EntityFrameworkCore.SqlServer.Tests

## Table of Contents <!-- omit in toc -->

- [Create the database](#create-the-database)
- [Check the database information](#check-the-database-information)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
dotnet tool restore
```

Set ConnectionStrings:

```shell
dotnet user-secrets set ConnectionStrings:ContosoUniversity "Data Source=sqlserver;Initial Catalog=ContosoUniversity;User ID=sa;Password=$(cat /run/secrets/db_password);Persist Security Info=False;TrustServerCertificate=yes"
```

Or Use environment variable `ConnectionStrings__ContosoUniversity`.

Add a Migration:

```shell
dotnet ef migrations add InitialCreate
```

Apply Migrations:

```shell
dotnet ef database update
```

## Check the database information

You can easily check this by using `sqlcmd` in the container.

```shell
./opt/mssql-tools18/bin/sqlcmd -U sa -C
```

> [!TIP]
> This is what happens, so "-C" is required.
>
> Sqlcmd: Error: Microsoft ODBC Driver 18 for SQL Server : SSL Provider: [error:0A000086:SSL routines::certificate verify failed:self-signed certificate].
Sqlcmd: Error: Microsoft ODBC Driver 18 for SQL Server : Client unable to establish connection. For solutions related to encryption errors, see <https://go.microsoft.com/fwlink/?linkid=2226722>.

Check the database list:

```SQL
SELECT name, database_id, create_date
FROM sys.databases;
GO
```

It's probably in the `master` database, so move it.

```SQL
USE Examples;
GO
```

Get a list of tables:

```SQL
SELECT schema_name(schema_id), name FROM sys.tables;
GO
```

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.EntityFrameworkCore.SqlServer.Tests
dotnet new xunit3 -o src/Examples.EntityFrameworkCore.SqlServer.Tests
dotnet sln add src/Examples.EntityFrameworkCore.SqlServer.Tests
cd src/Examples.EntityFrameworkCore.SqlServer.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector/
dotnet add package Microsoft.Data.SqlClient
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package Microsoft.Extensions.Configuration.UserSecrets

dotnet add reference ../Examples.EntityFrameworkCore/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated

dotnet new tool-manifest
dotnet tool install dotnet-ef
```
