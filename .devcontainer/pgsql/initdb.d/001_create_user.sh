#!/bin/bash

password=$(cat $MSSQL_SA_PASSWORD_FILE)

psql << EOT
SELECT version();
CREATE USER manager WITH encrypted password '${password}' CREATEDB;
CREATE USER operator WITH encrypted password '${password}';
\du
EOT
