# KiranaStore

# Deployment of Application

## Step 1
Download docker run time and run the `DockerCompose.yml` file by below command.

## Step 2
<small>**Note** : Before running this file make sure docker is running.</small>
### To run docker compose file follow below command.

#### Note: run the images in `DockerCompose.yml` file
```bash
docker-compose -f DockerCompose.yml up -d
```
#### Note: stop the running container in DockerCompose.yml file.
```bash
docker-compose -f DockerCompose.yml down
```