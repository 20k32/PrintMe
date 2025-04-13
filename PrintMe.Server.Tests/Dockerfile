FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src

COPY ["PrintMe.Server.Tests/PrintMe.Server.Tests.csproj", "./PrintMe.Server.Tests/"]
COPY ["PrintMe.Server/PrintMe.Server.csproj", "./PrintMe.Server/"]

RUN dotnet restore "./PrintMe.Server.Tests/PrintMe.Server.Tests.csproj"

COPY . .
WORKDIR "/src/PrintMe.Server.Tests"

ENTRYPOINT ["dotnet", "test", "--verbosity", "minimal"]