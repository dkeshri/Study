# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY . .
RUN dotnet restore "DataSync.DbChangeReceiverApp.csproj"
RUN dotnet publish "DataSync.DbChangeReceiverApp.csproj" -c release -o /consoleapp --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /consoleapp .

ENTRYPOINT [ "dotnet", "DataSync.DbChangeReceiverApp.dll" ]