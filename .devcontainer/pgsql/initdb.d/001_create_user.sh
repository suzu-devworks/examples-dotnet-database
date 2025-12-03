#!/bin/bash

password=$(cat /run/secrets/db_password)

psql << EOT
SELECT version();
CREATE USER manager WITH PASSWORD '${password}' CREATEDB;
CREATE USER operator WITH PASSWORD '${password}';
GRANT pg_signal_backend TO manager;
\du
EOT
