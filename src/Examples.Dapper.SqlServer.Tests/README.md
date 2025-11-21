# Examples.Dapper.SqlServer.Tests

## Table of Contents <!-- omit in toc -->

- [Connection String](#connection-string)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Connection String

- [SqlConnection.ConnectionString Property (Microsoft.Data.SqlClient) | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlconnection.connectionstring)

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
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
dotnet add package Dapper
dotnet add package Microsoft.Data.SqlClient

dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/
dotnet add reference ../Examples.Dapper/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
