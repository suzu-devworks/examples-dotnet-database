#!/bin/bash

password=$(cat $POSTGRES_PASSWORD_FILE)

psql << EOT
SELECT version();
CREATE USER manager WITH PASSWORD '${password}' CREATEDB;
CREATE USER operator WITH PASSWORD '${password}';
\du
EOT
