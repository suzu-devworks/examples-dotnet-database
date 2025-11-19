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
