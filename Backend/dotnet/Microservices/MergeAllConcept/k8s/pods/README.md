# Pod in kuberneties

It is smallest deployable component/resource in kubernetes.


## Deploy resource in namespace `dkeshri`

1. Create namespace 
    ```bash
    kubectl create namespace dkeshri
    ```
2. Apply
    ```bash
    kubectl apply -f .\order-api-pod.yaml
    ```
3. Port forwaring to local machine
    ```bash
    kubectl port-forward pod/order-api-pod 3000:8080 -n dkeshri
    ```