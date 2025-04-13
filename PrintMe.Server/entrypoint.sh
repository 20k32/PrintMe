#!/bin/bash
set -e

echo "Running database migrations..."
./efbundle

echo "Starting application..."
exec dotnet PrintMe.Server.dll