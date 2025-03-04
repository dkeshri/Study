# Kubernetes

Kubernetes is an open-source container orchestration platform that automates the deployment, scaling, and management of containerized applications. It was originally developed by Google and is now maintained by the Cloud Native Computing Foundation (CNCF).

## Why is Kubernetes Important?

* **Automated Scaling** – Kubernetes can automatically scale applications up or down based on demand, ensuring efficient resource utilization.
* Self-Healing – If a container or node fails, Kubernetes automatically replaces or restarts it, maintaining application availability.
* **Load Balancing & Service Discovery** – It distributes traffic evenly across multiple instances of an application, improving performance and reliability.
* **Declarative Configuration & Automation** – Using YAML files, developers can define the desired state of an application, and Kubernetes ensures it remains in that state.
* **Multi-Cloud & Hybrid Cloud Support** – Kubernetes works across different cloud providers and on-premises environments, making it highly flexible.
* **Rolling Updates & Rollbacks** – It allows for smooth updates without downtime and can revert to a previous version if needed.
* **Resource Efficiency** – Kubernetes optimizes infrastructure utilization, reducing costs by running applications efficiently on available hardware.

Overall, Kubernetes is essential for modern cloud-native applications, enabling developers and organizations to deploy and manage applications at scale with high resilience and automation.

## Setup Kubernetes locally 

1. **Enable Kubernetes in Docker Desktop**
    1. Open Docker Desktop.
    2. Go to Settings > Kubernetes.
    3. Check Enable Kubernetes and wait for it to start.

2. **Create Docker Images for Your Microservices**

    Ensure each microservice has a Dockerfile and build the images.

    For example, in each microservice folder, create a Dockerfile like this:

    Example `Dockerfile` for `OrderService`
    * [OrderService](../OrderService/Dockerfile)
    * [PaymentServive](../PaymentService/Dockerfile)
    * [InventoryService](../InventeryService/Dockerfile)
    * [ApiGatewayService](../ApiGateway/Dockerfile)
    * [AuthService](../AuthService/Dockerfile)
    * [OrchestratorService](../Orchestrator/Dockerfile)

    >Then build and tag the image, Make sure to Run Image Build commamd from **Parent Directory of K8s** -> `MergeAllConcept` directory.

    ```bash
    docker build -f ./OrderService/Dockerfile -t dkeshri/microservice-order-service:latest .
    ```
    > Repeat this for other services. like Payment,Inventory, Auth, ApiGateway and Orchestration Services.

3. **Write Kubernetes Deployment & Service Files**

    > We are using namespace dkeshri in all below deployment resources. so please use `-n dkeshri` with each kubectl command. or move current context from `default` to `dkeshri` namespace. 

    Create a `deployments` directory and define deployment resources YAML files.

    * [apigateway-deployment.yaml](./deployments/apigateway-deployment.yaml)
    * [auth-api-deployment.yaml](./deployments/auth-api-deployment.yaml)
    * [orchestrator-deployment.yaml](./deployments/orchestrator-deployment.yaml)
    * [order-api-deployment.yaml](./deployments/order-api-deployment.yaml)
    * [payment-api-deployment.yaml](./deployments/payment-api-deployment.yaml)
    * [inventory-api-deployment.yaml](./deployments/inventory-api-deployment.yaml)
    * [mssql-db-statefulset.yaml](./deployments/mssql-db-statefulset.yaml)
    * [rabbitmq-deployment.yaml](./deployments/rabbitmq-deployment.yaml)

4. **Deploy Services to Kubernetes**
    
    1. *Apply the deployments*:
    > Make sure to run below command from **Parent Directory** of `deployments` -> `k8s` directory.

    To Deploy all resources run below command.

    ```bash
    kubectl apply -f ./deployments
    ```
    If you are in current directoy of resources.yaml files then you can run the command as below

    ```bash
    kubectl apply -f .
    ```

    2. *Check if pods are running*:
    ```bash
    kubectl get pods -n dkeshri
    ```
    3. *Check services*:
    ```bash
    kubectl get services -n dkeshri
    ```
Your services should now be running and scaled as specified.

## Next Steps

* Configure Ingress to expose your API Gateway externally.
* Set up Persistent Volume if your services need data storage.
* Use ConfigMaps & Secrets for environment variables.
* Implement Horizontal Pod Autoscaler (HPA) for dynamic scaling.

### Set Up Ingress for API Gateway

**Install Ingress Controller (if not installed)**

Since you're using Docker Desktop's Kubernetes. you need an Ingress controller like **Nginx**.

*Run:*

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/deploy.yaml
```
*Verify installation:*
```bash
kubectl get pods -n ingress-nginx
```

### Create an Ingress Resource
*Create an Ingress Resource*

Save as [apigateway-ingress.yaml](./apigateway-ingress.yaml)


*Modify Your Hosts File (Windows)*

Edit the hosts file to map `apigateway.local` to `127.0.0.1`

1. Open Notepad as Administrator.
2. Open C:\Windows\System32\drivers\etc\hosts.
3. Add this line
```bash
127.0.0.1 apigateway.local
```
4. Save and close.

Now, you can access API Gateway at [http://apigateway.local.](http://apigateway.local)


### Set Up RabbitMQ in Kubernetes

*Deploy RabbitMQ*

[rabbitmq-deployment.yaml](./deployments/rabbitmq-deployment.yaml)

Apply it

```bash
kubectl apply -f ./deployments/rabbitmq-deployment.yaml
```

**Access RabbitMQ Dashboard**
Forward the `RabbitMQ` management port to your local machine:

```bash
kubectl port-forward service/rabbitmq-service 15672:15672 -n dkeshri
```
Now, open [http://localhost:15672/](http://localhost:15672/) in your browser.

Username: **guest**
Password: **guest**


Smilarly You can Also Access You OrderService and Other Deployed Services

Example My OrderService is Exposed in Kuberneties on Port `80` as you mapped you OrderService ContainerPort `8080` in  [order-api-deployment.yaml](./deployments/order-api-deployment.yaml)

To Access on host machine you can access you Service to Kubernetes Expose only as below

```bash
kubectl port-forward service/orderservice-service 7222:80 
```
You can Access it on host machine on **Port**: `7222` of `locahost` as URL: [http://localhost:7222/swagger](http://localhost:7222/swagger)


### Update Microservices to Use RabbitMQ

*Modify appsettings.json in your microservices:*

```json
"RabbitMQ": {
  "Host": "rabbitmq-service",
  "Port": 5672,
  "Username": "guest",
  "Password": "guest"
}
```
*Rebuild your microservices and restart the Kubernetes deployment:*

Example: 
```bash
kubectl rollout restart deployment order-api -n dkeshri

kubectl rollout restart deployment orchestrator -n dkeshri
```

### Final Checks
Run
```bash
kubectl get pods -n dkeshri
```

**Ensure all services are Running.**
Access API Gateway: [http://apigateway.local](http://apigateway.local)
Access RabbitMQ: [http://localhost:15672](http://localhost:15672)

## Advance Setup

1. [Horizontal Pod Autoscaler (HPA)](./hpa/README.md)
2. [Logging & Monitoring in Kubernetes with Prometheus and Grafana](./monitoring/README.md)
3. [Namespace in Kubernetes](./namespace/README.md)
4. [Commands in Kubernetes](./commands/README.md)