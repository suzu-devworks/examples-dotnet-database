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

## Examples.EntityFrameworkCore
dotnet new classlib -o src/Examples.EntityFrameworkCore
dotnet sln add src/Examples.EntityFrameworkCore
cd src/Examples.EntityFrameworkCore

dotnet add package Microsoft.EntityFrameworkCore.Relational

cd ../../

## Examples.ContosoUniversity
## Examples.Xunit

## Examples.EntityFrameworkCore.Tests
dotnet new xunit -o src/Examples.EntityFrameworkCore.Tests
dotnet sln add src/Examples.EntityFrameworkCore.Tests
cd src/Examples.EntityFrameworkCore.Tests

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.EntityFrameworkCore.InMemory

dotnet add reference src/Examples.EntityFrameworkCore/
dotnet add reference src/Examples.ContosoUniversity/
dotnet add reference src/Examples.Xunit/

cd ../../

# Update outdated package
dotnet list package --outdated
```
