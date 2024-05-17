# IOT Concept
## Table of content

* Background Services
    * [HostedService](./BackgroundApplication/MyHostedService.cs)
    * [ScheduledHostedService or HostedTimerService](./BackgroundApplication/ScheduledHostedService.cs)
    * [RunConsoleApplication As Lifelong](./BackgroundApplication/Program.cs)
    * [Use of Cancellation Token in Service](./BackgroundApplication/HostedTimerService.cs)
* Message Queue
    * [RabbitMq](./MessageQueue/MessageQueue.RabbitMq/README.md)

* Metrics Collector
    * [Prometheus](./MessageQueue/MessageQueue.WebApi/README.md)
        * [How to use prometheus](./MessageQueue/MessageQueue.WebApi/Program.cs)
    * [Web APP Docker Image](Frontend/web-app/README.md)




# Deployment of Application

## Step 1
Install docker run time and run the `DockerCompose.yaml` file by below command.

## Step 2

### To run docker compose file follow below command.

#### Note: run the images in `DockerCompose.yaml` file
```bash
docker-compose -f DockerCompose.yaml up -d
```
#### Note: stop the running container in DockerCompose.yaml file.
```bash
docker-compose -f DockerCompose.yaml down
```

### Command to run Keycloak
```bash
kc.bat start --http-enabled=true --https-certificate-file=D:\dkeshri\cert\keycloakSSL.pem --https-certificate-key-file=D:\dkeshri\cert\keycloak_private.pemp
```

