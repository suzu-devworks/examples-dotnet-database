# Examples.Dapper.PostgreSQL.Tests

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

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
dotnet add package Npgsql

dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity.Dapper/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```

## References

- [Welcome To Learn Dapper ORM - A Dapper Tutorial for C# and .NET Core](https://www.learndapper.com/dapper-query/selecting-scalar-values)
- [DapperLib/Dapper: Dapper - a simple object mapper for .Net](https://github.com/DapperLib/Dapper)
