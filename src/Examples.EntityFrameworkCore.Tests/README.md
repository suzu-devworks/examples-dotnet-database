# Examples.EntityFrameworkCore.Tests

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.EntityFrameworkCore.Tests
dotnet new xunit3 -o src/Examples.EntityFrameworkCore.Tests
dotnet sln add src/Examples.EntityFrameworkCore.Tests
cd src/Examples.EntityFrameworkCore.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

dotnet add reference ../Examples.EntityFrameworkCore/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated
```
