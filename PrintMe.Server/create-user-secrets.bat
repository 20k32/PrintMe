@ECHO OFF

dotnet user-secrets init

set /p jwtKey="Enter private jwt token without whitespaces: "
dotnet user-secrets set "PrivateJWTKey" %jwtKey%
dotnet user-secrets list

PAUSE