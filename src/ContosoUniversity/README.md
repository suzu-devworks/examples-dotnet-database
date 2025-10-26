# ContosoUniversity

This repository is a sample library used in other test projects.

The following site was used as reference:

- [ASP.NET Core Razor Pages with Entity Framework Core - Tutorial 1/8](https://docs.microsoft.com/ja-jp/aspnet/core/data/ef-rp/intro)

## Table of Contents <!-- omit in toc -->

- [The data model](#the-data-model)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## The data model

The following sections create a data model:

```mermaid
erDiagram
  direction LR
  Course {
    int CourseID  PK
    string Title
    int Credits
  }
  Enrollment {
    int EnrollmentID  PK
    int CourseID
    int StudentID
    Grade Grade
  }
  Student {
    int ID  PK
    string LastName
    string FirstMidName
    DateTime EnrollmentDate
  }
  Course ||--o{ Enrollment : has
  Enrollment }o--|| Student : can
```

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## ContosoUniversity
dotnet new classlib -o src/ContosoUniversity
dotnet sln add src/ContosoUniversity
cd src/ContosoUniversity

# for Infrastructure layer
dotnet add package Microsoft.EntityFrameworkCore.Relational
cd ../../

# Update outdated package
dotnet list package --outdated
```
