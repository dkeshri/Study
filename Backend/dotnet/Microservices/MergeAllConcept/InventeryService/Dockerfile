# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ../Contract ./Contract
COPY ../InventeryService ./InventeryService
RUN dotnet restore "./InventeryService/InventeryService.csproj"
RUN dotnet publish "./InventeryService/InventeryService.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /webapi
COPY --from=build /app .
EXPOSE 5000 5001 80 443 8080
ENTRYPOINT [ "dotnet", "InventeryService.dll" ]