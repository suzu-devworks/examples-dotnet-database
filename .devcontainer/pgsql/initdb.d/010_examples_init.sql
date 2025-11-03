CREATE DATABASE examples;

CREATE USER dev
WITH
    encrypted password 'XwkYC$3MuZrc&T';

GRANT ALL privileges ON database examples TO dev;
