SELECT pg_terminate_backend(pg_catalog.pg_stat_activity.pid)
FROM pg_catalog.pg_stat_activity
WHERE pg_catalog.pg_stat_activity.datname = 'printme_db'
  AND pid <> pg_backend_pid();

DROP DATABASE IF EXISTS printme_db;

DO $$ 
BEGIN
  IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'printme_owner') THEN
    RAISE NOTICE 'Role printme_owner exists, dropping owned objects.';
    DROP OWNED BY printme_owner;
  ELSE
    RAISE NOTICE 'Role printme_owner does not exist, skipping DROP OWNED.';
  END IF;
END $$;
DROP ROLE IF EXISTS printme_owner;

CREATE ROLE printme_owner WITH
    LOGIN
    SUPERUSER
    INHERIT
    CREATEDB
    CREATEROLE
    REPLICATION
    PASSWORD :'password';

CREATE DATABASE printme_db
    WITH
    OWNER = printme_owner
    ENCODING = 'UTF8'
    LC_COLLATE = 'Ukrainian_Ukraine.utf8'
    LC_CTYPE = 'Ukrainian_Ukraine.utf8'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

GRANT ALL ON DATABASE printme_db TO printme_owner;
