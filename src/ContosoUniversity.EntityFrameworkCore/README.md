# ContosoUniversity.EntityFrameworkCore

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## ContosoUniversity.EntityFrameworkCore
dotnet new classlib -o src/ContosoUniversity.EntityFrameworkCore
dotnet sln add src/ContosoUniversity.EntityFrameworkCore
cd src/ContosoUniversity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add reference ../ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated
```
