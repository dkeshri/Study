# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY . .
RUN dotnet restore "./Store.WebApi/Store.WebApi.csproj" --disable-parallel
RUN dotnet publish "./Store.WebApi/Store.WebApi.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /webapi
COPY --from=build /app .
EXPOSE 5000 5001 80 443 8080
RUN apt-get update
RUN apt-get --yes install curl
RUN apt-get install -y libgdiplus
ENTRYPOINT [ "dotnet", "Store.WebApi.dll" ]