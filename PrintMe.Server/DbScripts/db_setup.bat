@echo off
setlocal EnableDelayedExpansion

:: Set variables
set "INSTALL_DIR=C:\Program Files\PostgreSQL\17"
set "INIT_SQL_FILE=.\printme_init.sql"
set "SCHEMA_SQL_FILE=.\printme_schema.sql"
set "DATA_SQL_FILE=.\printme_data.sql"
set "PASSWORD_FILE=.\password.txt"

:: Logging function through labels
call :log "Starting PostgreSQL setup script"

:: Get user inputs using PowerShell for secure password input
for /f "delims=" %%i in ('powershell -Command "$pwd = Read-Host 'Enter the PostgreSQL superuser password' -AsSecureString; [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($pwd))"') do set "POSTGRES_PASSWORD=%%i"

set /p "DB_USER=Enter the database user (press Enter for default 'printme_owner'): "
if "!DB_USER!"=="" set "DB_USER=printme_owner"

for /f "delims=" %%i in ('powershell -Command "$pwd = Read-Host 'Enter the database user password' -AsSecureString; [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($pwd))"') do set "DB_USER_PASSWORD=%%i"

:: Save password temporarily
echo !DB_USER_PASSWORD!> "!PASSWORD_FILE!"

:: Check PostgreSQL service
sc query postgresql-x64-17 > nul
if !ERRORLEVEL! EQU 0 (
    call :log "PostgreSQL service is running"
) else (
    call :log "PostgreSQL service is not running"
    choice /C YN /M "Do you want to start PostgreSQL service"
    if !ERRORLEVEL! EQU 1 (
        net start postgresql-x64-17
        if !ERRORLEVEL! EQU 0 (
            call :log "PostgreSQL service started successfully"
        ) else (
            call :log "Failed to start PostgreSQL service"
            goto :cleanup
        )
    ) else (
        call :log "Service start was cancelled by user"
        goto :cleanup
    )
)

:: Run initialization SQL script as postgres user
call :log "Running initialization SQL script..."
set "PGPASSWORD=%POSTGRES_PASSWORD%"
"%INSTALL_DIR%\bin\psql.exe" -h localhost -p 5432 -U postgres -v password="%DB_USER_PASSWORD%" -f "%INIT_SQL_FILE%"

:: Run schema SQL script as printme_owner
call :log "Running schema SQL script..."
set "PGPASSWORD=%DB_USER_PASSWORD%"
"%INSTALL_DIR%\bin\psql.exe" -h localhost -p 5432 -U printme_owner -d printme_db -f "%SCHEMA_SQL_FILE%"

:: Run data SQL script as printme_owner
call :log "Running data SQL script..."
"%INSTALL_DIR%\bin\psql.exe" -h localhost -p 5432 -U printme_owner -d printme_db -f "%DATA_SQL_FILE%"

:cleanup
:: Cleanup
set "PGPASSWORD="
if exist "%PASSWORD_FILE%" del "%PASSWORD_FILE%"

echo.
echo Press any key to exit...
pause > nul
exit /b 0

:log
echo [%date% %time%] %~1
exit /b 0