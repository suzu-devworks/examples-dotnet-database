#!/bin/bash

if [ -n "${MSSQL_SA_PASSWORD_FILE}" ]; then
    MSSQL_SA_PASSWORD=$(cat $MSSQL_SA_PASSWORD_FILE)
    MSSQL_PASSWORD=$MSSQL_SA_PASSWORD
fi

SQLCMD=/opt/mssql-tools18/bin/sqlcmd
SQLCMD_SA="$SQLCMD -C -U sa -P $MSSQL_SA_PASSWORD"

function IsSqlServerReady
{
    IS_SERVER_READY_QUERY='SET NOCOUNT ON; Select SUM(state) from sys.databases'
    dbStatus=$($SQLCMD_SA -h -1 -Q "$IS_SERVER_READY_QUERY" 2>/dev/null)
    errCode=$?
    if [[ "$errCode" -eq "0" && "$dbStatus" -eq "0" ]]; then
        return 0
    else
        return 1
    fi
}

if [ -z "$MSSQL_CUSTOM_INIT" ]; then
    exit 0
fi

echo "Waiting for Sql Server to be ready before executing custom setup"
until IsSqlServerReady; do 
    sleep 5;
done

echo "Creating login with password defined in MSSQL_PASSWORD environment variable"
cmd="CREATE LOGIN manager WITH PASSWORD = '$MSSQL_PASSWORD';"
cmd+="CREATE LOGIN operator WITH PASSWORD = '$MSSQL_PASSWORD';"
$SQLCMD_SA -Q "$cmd"

if [ -n "${MSSQL_SETUP_SCRIPTS_LOCATION}" ]; then
    for file in $MSSQL_SETUP_SCRIPTS_LOCATION/*.sql; do
        echo "Executing custom setup script $file"
        $SQLCMD_SA -i $file
    done
fi
