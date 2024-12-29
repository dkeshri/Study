# Docker Commands

## Basic command
> Note: Before running commad Please Make Sure **Dockerfile** should be in current directory of terminal.

### Create Docker image locally

```bash
docker build -t nginx:latest .
```
> Note: To Create Image locally no need to provide `containerRegistry/UserName`

### Tag locally Image to ContainerRegistry (github,docker or azure or aws)

**For DockerHub**
```bash
docker tag nginx:latest USERNAME/REPOSITORY_NAME:TAG
```
**Example**
```bash
docker tag nginx:latest dkeshri/nginx:latest
```
**For GitHub**
```bash
docker tag nginx:latest ghcr.io/USERNAME/REPOSITORY_NAME:TAG
```
**Example**
```bash
docker tag nginx:latest ghcr.io/dkeshri/Nginx:latest
```

### Create Docker image for docker hub

> For dockerHub containerRegistry is optional, by default docker command set for dockerHub, only you need to provide dockerHub UseName. like `userName/ImageName:Tag` 

```bash
docker build -t dkeshri/nginx:latest .
```
### Push this image to docker hub.
```bash
docker push dkeshri/nginx:latest
```
> Note Here dkeshri is repository name of docker hub. we can aslo provide version in place of `latest` Tag like `1.2.0`

> For Other container registry like azure, AWS and github you need to provide `containerRegistry/userName/imageName:tag`

#### For Github Container registry (ghcr.io)

Build Image 
```bash
docker build -t ghcr.io/dkeshri/nginx:1.0.0 .
```
Push Image
```bash
docker push ghcr.io/dkeshri/nginx:1.0.0 .
```

