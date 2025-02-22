# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
# Copy the Contract project
COPY ../Contract ./Contract
COPY ../OrderService ./OrderService
RUN dotnet restore "./OrderService/OrderService.csproj"
RUN dotnet publish "./OrderService/OrderService.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /webapi
COPY --from=build /app .
EXPOSE 5000 5001 80 443 8080
ENTRYPOINT [ "dotnet", "OrderService.dll" ]