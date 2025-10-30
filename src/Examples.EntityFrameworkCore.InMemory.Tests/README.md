# Examples.EntityFrameworkCore.InMemory.Tests

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.EntityFrameworkCore.InMemory.Tests
dotnet new xunit3 -o src/Examples.EntityFrameworkCore.InMemory.Tests
dotnet sln add src/Examples.EntityFrameworkCore.InMemory.Tests
cd src/Examples.EntityFrameworkCore.InMemory.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.EntityFrameworkCore.InMemory

dotnet add reference ../Examples.EntityFrameworkCore/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated
```
