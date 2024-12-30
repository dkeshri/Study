# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ./DataSync/DataSync.Common ./DataSync/DataSync.Common
COPY ./DataSync/DataSync.DbChangeReceiver ./DataSync/DataSync.DbChangeReceiver
COPY ./MessageQueue/MessageQueue.RabbitMq ./MessageQueue/MessageQueue.RabbitMq


RUN dotnet restore "./DataSync/DataSync.DbChangeReceiver/DataSync.DbChangeReceiver.csproj"
RUN dotnet publish "./DataSync/DataSync.DbChangeReceiver/DataSync.DbChangeReceiver.csproj" -c release -o /consoleapp --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /consoleapp .

ENTRYPOINT [ "dotnet", "DataSync.DBChangeEmitter.dll" ]