# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ../DataSync.Common ./
COPY ../DataSync.DbChangeReceiver ./
RUN dotnet restore "./DataSync.DbChangeReceiver.DBChangeEmitter/DataSync.DbChangeReceiver.csproj" --disable-parallel
RUN dotnet publish "./DataSync.DbChangeReceiver/DataSync.DbChangeReceiver.csproj" -c release -o /consoleapp --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /consoleapp .

ENTRYPOINT [ "dotnet", "DataSync.DbChangeReceiver.dll" ]