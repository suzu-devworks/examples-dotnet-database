# Examples.Xunit

This library defines common and commonly used functions for Xunit. Once it is organized, it will be made into a Nuget package.

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Xunit
dotnet new classlib -o src/Examples.Xunit
dotnet sln add src/Examples.Xunit
cd src/Examples.Xunit
dotnet add package xunit.abstractions
dotnet add package Microsoft.Extensions.Logging.Debug
cd ../../

# Update outdated package
dotnet list package --outdated
```
