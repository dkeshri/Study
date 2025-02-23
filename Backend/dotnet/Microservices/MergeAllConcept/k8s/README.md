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

    > Create a k8s folder and define YAML files.
    Deployment for OrderService

    * [apigateway-deployment.yaml](./apigateway-deployment.yaml)
    * [authservice-deployment.yaml](./authservice-deployment.yaml)
    * [orchestrator-deployment.yaml](./orchestrator-deployment.yaml)
    * [orderservice-deployment.yaml](./orderservice-deployment.yaml)
    * [paymentservice-deployment.yaml](./paymentservice-deployment.yaml)
    * [inventoryservice-deployment.yaml](./inventoryservice-deployment.yaml)
    * [mssql-deployment.yaml](./mssql-deployment.yaml)
    * [rabbitmq-deployment.yaml](./rabbitmq-deployment.yaml)

4. **Deploy Services to Kubernetes**
    
    1. *Apply the deployments*:
    > Make sure to run below command from **Parent Directory** of `k8s` -> `MergeAllConcept` directory.
    ```bash
    kubectl apply -f ./k8s
    ```
    If you are in current directoy of deployments.yaml files then you can run the command as below

    ```bash
    kubectl apply -f .
    ```

    2. *Check if pods are running*:
    ```bash
    kubectl get pods
    ```
    3. *Check services*:
    ```bash
    kubectl get services
    ```
Your services should now be running and scaled as specified.

## Next Steps

* Configure Ingress to expose your API Gateway externally.
* Set up Persistent Volume if your services need data storage.
* Use ConfigMaps & Secrets for environment variables.
* Implement Horizontal Pod Autoscaler (HPA) for dynamic scaling.

### Set Up Ingress for API Gateway

**Install Ingress Controller (if not installed)**

Since you're using Docker Desktop's Kubernetes, you need an Ingress controller like **Nginx**.

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

[rabbitmq-deployment.yaml](./rabbitmq-deployment.yaml)

Apply it

```bash
kubectl apply -f rabbitmq-deployment.yaml
```

**Access RabbitMQ Dashboard**
Forward the `RabbitMQ` management port to your local machine:

```bash
kubectl port-forward service/rabbitmq-service 15672:15672
```
Now, open [http://localhost:15672/](http://localhost:15672/) in your browser.

Username: **guest**
Password: **guest**


Smilarly You can Also Access You OrderService and Other Deployed Services

Example My OrderService is Exposed in Kuberneties on Port `80` as you mapped you OrderService ContainerPort `8080` in  [orderservice-deployment.yaml](./orderservice-deployment.yaml)

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

```bash
kubectl rollout restart deployment orderservice
kubectl rollout restart deployment orchestrator
```

### Final Checks
Run
```bash
kubectl get pods
```

**Ensure all services are Running.**
Access API Gateway: [http://apigateway.local](http://apigateway.local)
Access RabbitMQ: [http://localhost:15672](http://localhost:15672)



## Commands in Kubernetes

1. Apply change for one Service
    ```bash
    kubectl apply -f orderservice-deployment.yaml
    ```
2. Apply change to all Service
    Note: Make sure to Run below command from Parent directory of `k8s` i.e `MergeAllConcept`
    ```bash
    kubectl apply -f ./k8s
    ```
    If you want to apply current directort then use `.` instade for k8s directory
    ```bash
    kubectl apply -f .
    ```
3. Delete all the Deployment
    ```bash
    kubectl delete all --all --grace-period=0 --force   
    ```
4. Get Pod details
    ```bash
    kubectl get pods -o wide 
    ```
5. Get Service details
    ```bash
    kubectl get services -o wide
    ```
6. Get everything
    ```bash
    kubectl get all
    ```
7. Access Specefic Service on host machine
    like order service

    ```bash
    kubectl port-forward service/orderservice-service 7222:80 
    ```
    You can Access it on host machine on **Port**: `7222` of `locahost` as URL: [http://localhost:7222/swagger](http://localhost:7222/swagger)