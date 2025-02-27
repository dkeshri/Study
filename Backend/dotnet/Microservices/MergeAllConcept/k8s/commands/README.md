# Commands in Kubernetes

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



## Delete 

**Delete all from namespce**

namespace : dkeshri

```bash
kubectl delete all --all -n dkeshri
```
* This deletes:

    `Pods, Services, Deployments,StatefulSets,DaemonSets, ReplicaSets, PersistentVolumeClaims (PVCs), ConfigMaps, Secrets,
 Other resource types`


**Command	Description**

| Parameter                    | Description     |
|----------------------------- |-----------------|
| `kubectl get pods -o wide` | 	Show where pods are running  |
| `kubectl get pod <pod-name> -o wide`|	Show node of a specific pod|
| `kubectl get pods -A -o wide`|   See all pods and where they are running |	
| `kubectl get pods -n kube-system`|	Check core Kubernetes system components|
| `kubectl top pods -A`	|   Show resource usage per pod|
| `kubectl get nodes -o wide`  | Show all nodes (master & workers) |
| `kubectl describe node <node-name>`|	Show node details|
| `kubectl get services -o wide`    | 	Show service details   |
| `kubectl cluster-info`|	Check if cluster is running|
| `kubectl get nodes -o wide`|  Check node status (master & workers)|
| `kubectl get events --sort-by=.metadata.creationTimestamp`|	Show cluster events (errors, warnings)|
| `kubectl get componentstatuses`|	Check Kubernetes component health|
| `kubectl top nodes`|	Show resource usage per node|
| `kubectl get hpa`|    Check the horizontal-pod-autoscale status|
| `kubectl edit daemonset monitoring-prometheus-node-exporter -n monitoring`|   Edit the damonset for monitoring-prometheus-node-exporter |
| `kubectl get ds -n monitoring`|   Lists `DaemonSets` in the monitoring namespace.|