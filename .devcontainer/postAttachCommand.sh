#!/bin/sh
echo "USER:" `whoami`

if [ -n "$MSSQL_SERVICE" ]; then
echo "SERVICE:" $MSSQL_SERVICE
dotnet user-secrets --project src/Examples.EntityFrameworkCore.SqlServer.Tests set ConnectionStrings:ContosoUniversity "Data Source=${MSSQL_SERVICE};Initial Catalog=ContosoUniversity;User ID=sa;Password=$(cat /run/secrets/db_password);Persist Security Info=False;TrustServerCertificate=yes"
dotnet user-secrets --project src/Examples.EntityFrameworkCore.SqlServer.Tests list
fi

if [ -n "$POSTGRES_SERVICE" ]; then
echo "SERVICE:" $POSTGRES_SERVICE
dotnet user-secrets --project src/Examples.EntityFrameworkCore.PostgreSQL.Tests set ConnectionStrings:ContosoUniversity "Host=${POSTGRES_SERVICE};Database=contoso;Username=postgres;Password=$(cat /run/secrets/db_password)"
dotnet user-secrets --project src/Examples.EntityFrameworkCore.PostgreSQL.Tests list
fi
