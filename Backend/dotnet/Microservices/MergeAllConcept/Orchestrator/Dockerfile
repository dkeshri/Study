# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ../Contract ./Contract
COPY ../Orchestrator ./Orchestrator
RUN dotnet restore "./Orchestrator/Orchestrator.csproj"
RUN dotnet publish "./Orchestrator/Orchestrator.csproj" -c release -o /consoleapp --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /consoleapp .

ENTRYPOINT [ "dotnet", "Orchestrator.dll" ]