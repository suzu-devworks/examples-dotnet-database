#!/bin/sh
echo "USER:" `whoami`

# add xunit3 template
dotnet new install xunit.v3.templates

# set password
password=$(cat /run/secrets/db_password)

if [ -n "$MSSQL_SERVICE" ]; then
echo "SERVICE: [$MSSQL_SERVICE]" 
dotnet user-secrets --project src/Examples.EntityFrameworkCore.SqlServer.Tests \
    set ConnectionStrings:ContosoUniversity "Data Source=${MSSQL_SERVICE};Initial Catalog=ContosoUniversity;User ID=sa;Password=${password};Persist Security Info=False;TrustServerCertificate=yes"
dotnet user-secrets --project src/Examples.EntityFrameworkCore.SqlServer.Tests list
fi

if [ -n "$POSTGRES_SERVICE" ]; then
echo "SERVICE:" $POSTGRES_SERVICE
dotnet user-secrets --project src/Examples.EntityFrameworkCore.PostgreSQL.Tests \
    set ConnectionStrings:ContosoUniversity "Host=${POSTGRES_SERVICE};Database=contoso_university;Username=postgres;Password=${password}"
dotnet user-secrets --project src/Examples.EntityFrameworkCore.PostgreSQL.Tests list

dotnet user-secrets --project src/Examples.Dapper.PostgreSQL.Tests \
    set ConnectionStrings:ContosoUniversity "Host=${POSTGRES_SERVICE};Database=contoso_university;Username=postgres;Password=${password}"
dotnet user-secrets --project src/Examples.Dapper.PostgreSQL.Tests \
    set ConnectionStrings:ProductCatalogs "Host=${POSTGRES_SERVICE};Database=product_catalogs;Username=operator;Password=${password}"
dotnet user-secrets --project src/Examples.Dapper.PostgreSQL.Tests list
fi
