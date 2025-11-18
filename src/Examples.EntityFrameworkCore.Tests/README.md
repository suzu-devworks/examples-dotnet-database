# Examples.EntityFrameworkCore.Tests

## Table of Contents <!-- omit in toc -->

- [Examples.EntityFrameworkCore Tests](#examplesentityframeworkcore-tests)
- [ContosoUniversity Tests](#contosouniversity-tests)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Examples.EntityFrameworkCore Tests

- [Metadata/](./EntityFrameworkCore.Tests/Metadata/)

## ContosoUniversity Tests

- [Repositories/](./EntityFrameworkCore.InMemory.Tests/ContosoUniversity/Repositories/)

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

dotnet add reference ../Examples.EntityFrameworkCore/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated
```
