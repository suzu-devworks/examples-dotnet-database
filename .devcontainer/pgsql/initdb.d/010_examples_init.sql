CREATE DATABASE examples;

CREATE USER dev
WITH
    encrypted password 'XwkYC$3MuZrc&T';

GRANT ALL privileges ON database examples TO dev;

/* sql-formatter-disable */
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO dev;

/* sql-formatter-enable */
