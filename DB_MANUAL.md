# Local database setup

## Requirements

First of all, you have to download and install PostgreSQL 17 [here](https://www.enterprisedb.com/postgresql-tutorial-resources-training-1?uuid=69f95902-b451-4735-b7e4-1b62209d4dfd&campaignId=postgres_rc_17)

Set default installation path to `C:\Program Files\PostgreSQL\17` and data directory to `C:\Program Files\PostgreSQL\17\data`.

**During installation, you have to set a password for the default user `postgres`. Remember it, you will need it later.**

## Database setup

After installation, you have to create a new database and user. You can just run the appropriate script from the project root directory: `DbScripts/db_setup.bat`.

## Rider setup

Now you have to set up the connection to the database in the project. Open the project in Rider, then go to View -> Tool Windows -> Database. Click on the plus icon in the top left corner, then select Connect to Database -> Use connection string. In the URL field, type `jdbc:postgresql://localhost:5432/printme_db`, in the User field type `printme_owner` (it can be another), in the Password field type the password you set for this user. Click Test Connection, if everything is correct, you should see a success message. Click OK.

## Useful links

[PostgreSQL documentation](https://www.postgresql.org/docs/current/index.html)
