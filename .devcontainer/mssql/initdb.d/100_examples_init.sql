/* sql-formatter-disable */
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'examples')
BEGIN
    CREATE DATABASE examples;
END

/* sql-formatter-enable */
SELECT
    name
FROM
    sys.databases;

GO
-- ----- 
USE examples;

/* sql-formatter-disable */
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'manager' AND type = 'S')
BEGIN
    CREATE USER manager FOR LOGIN manager;
    ALTER ROLE db_ddladmin ADD MEMBER manager;
END

/* sql-formatter-disable */
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'operator' AND type = 'S')
BEGIN
    CREATE USER operator FOR LOGIN operator;
    ALTER ROLE db_datareader ADD MEMBER operator;
    ALTER ROLE db_datawriter ADD MEMBER operator;
END

/* sql-formatter-enable */
SELECT
    roles.principal_id AS roleprincipalid,
    roles.name AS roleprincipalname,
    database_role_members.member_principal_id AS memberprincipalid,
    members.name AS memberprincipalname
FROM
    sys.database_role_members AS database_role_members
    INNER JOIN sys.database_principals AS roles ON database_role_members.role_principal_id = roles.principal_id
    INNER JOIN sys.database_principals AS members ON database_role_members.member_principal_id = members.principal_id;

GO
