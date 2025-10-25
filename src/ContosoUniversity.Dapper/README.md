# ContosoUniversity.Dapper

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## ContosoUniversity.Dapper
dotnet new classlib -o src/ContosoUniversity.Dapper
dotnet sln add src/ContosoUniversity.Dapper
cd src/ContosoUniversity.Dapper
dotnet add package Microsoft.Extensions.Logging.Abstractions
dotnet add package Dapper

dotnet add reference ../ContosoUniversity/
cd ../../

# Update outdated package
dotnet list package --outdated
```
