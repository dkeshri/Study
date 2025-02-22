# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ../AuthService ./AuthService
RUN dotnet restore "./AuthService/AuthService.csproj"
RUN dotnet publish "./AuthService/AuthService.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /webapi
COPY --from=build /app .
EXPOSE 5000 5001 80 443 8080
ENTRYPOINT [ "dotnet", "AuthService.dll" ]