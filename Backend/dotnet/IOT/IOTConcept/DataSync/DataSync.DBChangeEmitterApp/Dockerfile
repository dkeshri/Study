# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ./DataSync/DataSync.Common ./DataSync/DataSync.Common
COPY ./DataSync/DataSync.DBChangeEmitter ./DataSync/DataSync.DBChangeEmitter
COPY ./MessageQueue/MessageQueue.RabbitMq ./MessageQueue/MessageQueue.RabbitMq


RUN dotnet restore "./DataSync/DataSync.DBChangeEmitter/DataSync.DBChangeEmitter.csproj"
RUN dotnet publish "./DataSync/DataSync.DBChangeEmitter/DataSync.DBChangeEmitter.csproj" -c release -o /consoleapp --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /consoleapp .

ENTRYPOINT [ "dotnet", "DataSync.DBChangeEmitter.dll" ]