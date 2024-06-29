# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY . .
RUN dotnet restore "./TestApp.csproj" --disable-parallel
RUN dotnet publish "./TestApp.csproj" -c Release -o /out --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /out .
# Install the debugger tools
RUN apt-get update \
    && apt-get install -y unzip \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

ENTRYPOINT [ "dotnet", "TestApp.dll" ]