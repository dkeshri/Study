# Study
## Table of content

* Backend
    * [Migration EF core](Backend/README.md)
    * [Web API Docker Image creation](Backend/Store.WebServer/README.md)

* Frontend
    * [Web App](Frontend/README.md)
    * [Web APP Docker Image](Frontend/web-app/README.md)

* [SSL Certificate](Certificate/README.md)



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

