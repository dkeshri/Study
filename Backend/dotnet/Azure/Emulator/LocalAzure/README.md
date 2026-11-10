# Local Azure Development Setup

Run Azure Service Bus (queues, topics, subscriptions) and Azure Storage locally with Docker Desktop, without an Azure subscription.

This setup is intended for **local development and learning**, not production.

## 1. What this setup runs

| Component | Purpose | Local endpoint |
|---|---|---|
| SQL Server 2022 | Storage dependency for Service Bus Emulator | Container-only |
| Azure Service Bus Emulator | Queues, topics, subscriptions and message operations | `localhost:5672` (AMQP), `localhost:5300` (management/health) |
| Azurite | Local Azure Storage emulator for Functions development | `localhost:10000` (Blob), `localhost:10001` (Queue), `localhost:10002` (Table) |

Your .NET applications and Azure Functions can run directly on Windows, outside Docker.

> **Important:** The Service Bus Emulator is not the production Azure Service Bus service. It has feature and scale limitations. It does not emulate Azure Event Grid.

## 2. Prerequisites

Install:

1. [Docker Desktop](https://www.docker.com/products/docker-desktop/) with the WSL 2 backend on Windows.
2. [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
3. [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local) if you want to run Functions locally.
4. PowerShell (Windows) or a compatible shell.

Verify Docker:

```powershell
docker --version
docker compose version
```

Make sure Docker Desktop is running before continuing.

## 3. Folder structure

Keep the Service Bus configuration under `Configs` and name it `order-config.json`:

```text
LocalAzure/
├── docker-compose.yml
├── .env
├── .gitignore
└── Configs/
    └── order-config.json
```

Run Docker Compose commands from the `LocalAzure` directory (the directory containing `docker-compose.yml`).

## 4. Configure Service Bus entities

The file `Configs/order-config.json` defines the queues, topics and subscriptions created by the emulator.

Example `Configs/order-config.json`:

```json
{
  "UserConfig": {
    "Namespaces": [
      {
        "Name": "sbemulatorns",
        "Queues": [
          {
            "Name": "order-queue",
            "Properties": {
              "DefaultMessageTimeToLive": "PT1H",
              "LockDuration": "PT1M",
              "MaxDeliveryCount": 5,
              "RequiresDuplicateDetection": false,
              "RequiresSession": false,
              "DeadLetteringOnMessageExpiration": true
            }
          }
        ],
        "Topics": [
          {
            "Name": "order-events",
            "Properties": {
              "DefaultMessageTimeToLive": "PT1H",
              "RequiresDuplicateDetection": false
            },
            "Subscriptions": [
              {
                "Name": "inventory-sub",
                "Properties": {
                  "LockDuration": "PT1M",
                  "MaxDeliveryCount": 5,
                  "RequiresSession": false
                }
              },
              {
                "Name": "payment-sub",
                "Properties": {
                  "LockDuration": "PT1M",
                  "MaxDeliveryCount": 5,
                  "RequiresSession": false
                }
              },
              {
                "Name": "notification-sub",
                "Properties": {
                  "LockDuration": "PT1M",
                  "MaxDeliveryCount": 5,
                  "RequiresSession": false
                }
              }
            ]
          }
        ]
      }
    ],
    "Logging": {
      "Type": "Console"
    }
  }
}
```

This example creates:

- Queue: `order-queue`
- Topic: `order-events`
- Subscriptions: `inventory-sub`, `payment-sub`, `notification-sub`

The emulator reads this configuration at startup. When you change entity definitions, recreate the emulator container so it loads the updated configuration.

## 5. Docker Compose

Create `docker-compose.yml` in the `LocalAzure` directory:

```yaml
name: local-azure

services:
  mssql:
    container_name: mssql
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: ${ACCEPT_EULA}
      MSSQL_SA_PASSWORD: ${MSSQL_SA_PASSWORD}
    networks:
      - sb-network

  servicebus-emulator:
    container_name: servicebus-emulator
    image: mcr.microsoft.com/azure-messaging/servicebus-emulator:latest
    pull_policy: always
    volumes:
      - "${CONFIG_PATH}:/ServiceBus_Emulator/ConfigFiles/Config.json:ro"
    ports:
      - "5672:5672"
      - "5300:5300"
    environment:
      SQL_SERVER: mssql
      MSSQL_SA_PASSWORD: ${MSSQL_SA_PASSWORD}
      ACCEPT_EULA: ${ACCEPT_EULA}
      EMULATOR_HTTP_PORT: 5300
    depends_on:
      - mssql
    networks:
      - sb-network

  azurite:
    container_name: azurite
    image: mcr.microsoft.com/azure-storage/azurite
    command: >
      azurite
      --blobHost 0.0.0.0
      --queueHost 0.0.0.0
      --tableHost 0.0.0.0
      --location /data
      --debug /data/debug.log
    ports:
      - "10000:10000"
      - "10001:10001"
      - "10002:10002"
    volumes:
      - azurite-data:/data

networks:
  sb-network:
    driver: bridge

volumes:
  azurite-data:
```

**Important configuration mount:** `./Configs/order-config.json` is resolved relative to the `docker-compose.yml` file. Make sure the file exists at exactly that path. The container mounts it as `/ServiceBus_Emulator/ConfigFiles/Config.json`, which is the path expected by the emulator. `/ServiceBus_Emulator/ConfigFiles/Config.json:ro` `ro` stands for read only mode. The container can read the file but cannot modify it. 

The SQL Server container is intentionally not published to the host because the emulator reaches it over the Docker network. If you want to connect to SQL Server from your host for diagnostics, add a port mapping such as `1433:1433` and configure SQL Server to listen on that port.

## 6. Environment variables

Create `.env` in `LocalAzure`:

```dotenv
ACCEPT_EULA=Y
MSSQL_SA_PASSWORD=LocalDev@12345
```

Change the example password before use. SQL Server requires a strong password. Do not reuse a production password.

Create `.gitignore`:

```gitignore
.env
local.settings.json
bin/
obj/
```

## 7. Start the containers

Open PowerShell in the `LocalAzure` directory:

```powershell
docker compose pull
docker compose up -d
docker compose ps
```

View logs:

```powershell
docker compose logs -f servicebus-emulator
docker compose logs -f azurite
```

Check the emulator health endpoint:

```powershell
Invoke-RestMethod http://localhost:5300/health
```

If the emulator is still starting, wait and check its logs again.

### Recreate the emulator after changing the config

If you update `Configs/order-config.json`, recreate the emulator container:

```powershell
docker compose up -d --force-recreate servicebus-emulator
docker compose logs -f servicebus-emulator
```

If you want to recreate the entire local stack:

```powershell
docker compose down
docker compose up -d
```

Avoid `docker compose down -v` unless you intentionally want to delete the Azurite data volume too.

## 8. Service Bus connection string

For .NET applications running on the Windows host, use the emulator connection string:

```text
Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;
```

For a .NET application running inside the same Docker network, use the service DNS name instead of `localhost`:

```text
Endpoint=sb://servicebus-emulator;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;
```

The emulator's development connection string uses a well-known emulator key. This is only for local development.

### Install the .NET SDK package

```powershell
dotnet add package Azure.Messaging.ServiceBus
```

### Send a message to the queue

```csharp
using Azure.Messaging.ServiceBus;

var connectionString =
    "Endpoint=sb://localhost;" +
    "SharedAccessKeyName=RootManageSharedAccessKey;" +
    "SharedAccessKey=SAS_KEY_VALUE;" +
    "UseDevelopmentEmulator=true;";

await using var client = new ServiceBusClient(connectionString);
await using var sender = client.CreateSender("order-queue");

var message = new ServiceBusMessage("Order 1001 created")
{
    ContentType = "text/plain",
    MessageId = Guid.NewGuid().ToString()
};

await sender.SendMessageAsync(message);
```

### Send a message to a topic

```csharp
await using var topicSender = client.CreateSender("order-events");

var eventMessage = new ServiceBusMessage(
    """{"orderId":"1001","eventType":"OrderCreated"}""")
{
    ContentType = "application/json",
    MessageId = Guid.NewGuid().ToString()
};

await topicSender.SendMessageAsync(eventMessage);
```

A topic publishes a copy to each matching subscription. A queue is for competing consumers sharing the same work.

## 9. Azure Functions with Service Bus

For a .NET isolated Azure Functions project, install the Service Bus extension package appropriate for your Functions worker version. For current isolated-worker projects, use:

```powershell
dotnet add package Microsoft.Azure.Functions.Worker.Extensions.ServiceBus
```

Example `local.settings.json` for a Function running on the Windows host:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ServiceBusConnection": "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;"
  }
}
```

Example queue-triggered Function:

```csharp
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class OrderQueueFunction
{
    private readonly ILogger<OrderQueueFunction> _logger;

    public OrderQueueFunction(ILogger<OrderQueueFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(OrderQueueFunction))]
    public void Run(
        [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")]
        string message)
    {
        _logger.LogInformation("Received order message: {Message}", message);
    }
}
```

Example topic-subscription trigger:

```csharp
[Function(nameof(InventoryFunction))]
public void Run(
    [ServiceBusTrigger(
        "order-events",
        "inventory-sub",
        Connection = "ServiceBusConnection")]
    string message)
{
    _logger.LogInformation("Inventory received: {Message}", message);
}
```

Run the Function locally from its project directory:

```powershell
func start
```

`UseDevelopmentStorage=true` points Functions to Azurite for its host storage dependency. If the Function app itself runs inside Docker rather than on Windows, configure explicit Azurite endpoints using the Docker service name instead of `localhost`.

## 10. Azurite endpoints

| Storage service | Endpoint |
|---|---|
| Blob | `http://127.0.0.1:10000/devstoreaccount1` |
| Queue | `http://127.0.0.1:10001/devstoreaccount1` |
| Table | `http://127.0.0.1:10002/devstoreaccount1` |

For a host-based local application, the standard development storage connection string is:

```text
UseDevelopmentStorage=true
```

Azurite persists its data in the Docker volume named `azurite-data`.

## 11. Useful Docker commands

```powershell
# Check running containers
docker compose ps

# Follow all logs
docker compose logs -f

# Follow only Service Bus Emulator logs
docker compose logs -f servicebus-emulator

# Stop containers but retain named volumes
docker compose down

# Start again
docker compose up -d

# Pull the latest configured images
docker compose pull
```

## 12. Troubleshooting

### Service Bus Emulator does not become healthy

1. Check SQL Server and emulator logs:
   ```powershell
   docker compose logs mssql
   docker compose logs servicebus-emulator
   ```
2. Confirm Docker Desktop has enough memory allocated.
3. Confirm `.env` exists and the SQL Server password meets SQL Server complexity requirements.
4. Confirm `Configs/order-config.json` exists and contains valid JSON.
5. Confirm the Compose volume mapping is exactly:
   `./Configs/order-config.json:/ServiceBus_Emulator/ConfigFiles/Config.json:ro`

### Configuration changes are not reflected

Recreate the emulator container:

```powershell
docker compose up -d --force-recreate servicebus-emulator
```

### Connection refused

- Host applications should connect to `localhost:5672`.
- Containers on the Compose network should connect to `servicebus-emulator:5672`.
- Check that the ports are published and the emulator is healthy.

### Function cannot access AzureWebJobsStorage

- Confirm Azurite is running: `docker compose ps`.
- For Functions running on Windows, use `UseDevelopmentStorage=true`.
- For Functions running in Docker, use explicit Azurite endpoints with the Compose service DNS name.

## 13. Emulator limitations

The Service Bus Emulator is for development and testing, not production. It does not provide the full Azure Service Bus feature set or cloud operational capabilities. Limitations include no Entra ID authentication, no Azure portal integration, no cloud metrics/alerts, no geo-disaster recovery, and unsupported advanced features. It uses AMQP over TCP rather than AMQP WebSockets.

Validate the current supported-feature list in Microsoft's documentation before relying on a feature in local tests.

## 14. Official documentation

- [Azure Service Bus Emulator – test locally](https://learn.microsoft.com/azure/service-bus-messaging/overview-emulator)
- [Service Bus Emulator configuration](https://learn.microsoft.com/azure/service-bus-messaging/test-locally-with-service-bus-emulator)
- [Azurite documentation](https://learn.microsoft.com/azure/storage/common/storage-use-azurite)
- [Run Azure Functions locally](https://learn.microsoft.com/azure/azure-functions/functions-run-local)

## 15. Quick setup checklist

- [ ] Docker Desktop is running.
- [ ] `LocalAzure/Configs/order-config.json` exists.
- [ ] `.env` exists in `LocalAzure`.
- [ ] `docker-compose.yml` mounts `./Configs/order-config.json`.
- [ ] `docker compose up -d` completes.
- [ ] `http://localhost:5300/health` responds.
- [ ] .NET apps use the `localhost` Service Bus connection string.
- [ ] Functions use Azurite through `UseDevelopmentStorage=true`.
