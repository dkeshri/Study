# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Source
COPY ../Contract ./Contract
COPY ../PaymentService ./PaymentService
RUN dotnet restore "./PaymentService/PaymentService.csproj"
RUN dotnet publish "./PaymentService/PaymentService.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /webapi
COPY --from=build /app .
EXPOSE 5000 5001 80 443 8080
ENTRYPOINT [ "dotnet", "PaymentService.dll" ]