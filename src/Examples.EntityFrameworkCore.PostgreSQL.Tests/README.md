# Examples.EntityFrameworkCore.PostgreSQL.Tests

## Table of Contents <!-- omit in toc -->

- [Create the database](#create-the-database)
- [Development](#development)
  - [`Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported.`](#cannot-write-datetime-with-kindunspecified-to-postgresql-type-timestamp-with-time-zone-only-utc-is-supported)
  - [If the date type is still messing up](#if-the-date-type-is-still-messing-up)
  - [I usually stick to snake\_case when naming stuff in PostgreSQLs](#i-usually-stick-to-snake_case-when-naming-stuff-in-postgresqls)
  - [How the project was initialized](#how-the-project-was-initialized)

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
dotnet tool restore
```

Set ConnectionStrings:

```shell
dotnet user-secrets set ConnectionStrings:ContosoUniversity "Host=postgres;Database=postgres;Username=postgres;Password=$(cat /run/secrets/db_password)"
```

Or Use environment variable `ConnectionStrings__ContosoUniversity`.

Add a Migration:

```shell
dotnet ef migrations add InitialCreate
```

Apply Migrations:

```shell
dotnet ef database update
```

## Development

### `Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported.`

As stated, in order to register data as a timestamp with time zone, the `DateTime`'s `DateTimeKind` must be `UTC`.

If you think about it carefully, there is no DateTime type in .NET that stores TimeZone. It seems that the mapping is insufficient.

The optimal solution for PostgreSQL seems to be to use a library called NodaTime, but if you only need to convert to UTC, there are probably other ways to solve the problem. One such solution is `ValueConverter`.

```cs
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .Property(x => x.EnrollmentDate)
            .HasConversion<UniversalDateTimeConvertor>();
    }
```

### If the date type is still messing up

I haven't started researching it yet.

- [Date/Time Mapping with NodaTime | Npgsql Documentation](https://www.npgsql.org/efcore/mapping/nodatime.html?tabs=ef9-with-connection-string)

### I usually stick to snake_case when naming stuff in PostgreSQLs

Change the naming convention for EF Core's auto-mapping.

```shell
dotnet add package EFCore.NamingConventions
```

Enable a naming convention in your model's OnConfiguring method:

```cs
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseNpgsql(...)
        .UseSnakeCaseNamingConvention();
```

If you change this, the migration table(`__EFMigrationsHistory`) will also change, so you will need to recreate the migration.

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.EntityFrameworkCore.PostgreSQL.Tests
dotnet new xunit3 -o src/Examples.EntityFrameworkCore.PostgreSQL.Tests
dotnet sln add src/Examples.EntityFrameworkCore.PostgreSQL.Tests
cd src/Examples.EntityFrameworkCore.PostgreSQL.Tests

dotnet add package xunit.v3.mtp-v2
dotnet add package Microsoft.Testing.Extensions.CodeCoverage
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime
dotnet add package EFCore.NamingConventions

dotnet add reference ../Examples.EntityFrameworkCore/
dotnet add reference ../Examples.Various/
dotnet add reference ../ContosoUniversity/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated

dotnet new tool-manifest
dotnet tool install dotnet-ef
```
