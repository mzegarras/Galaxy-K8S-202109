## Instalar Google Cloud sdk
1. SDK Google [SDK Google] https://cloud.google.com/sdk/docs/install

## Crear Proyecto

1. Crear proyecto: k8staller
1. Seleccionar: Kubernetes Engine
1. Habilitar API

## Instalar Kubernetes Cli
1. 1. Kubernetes [SDK Google]  https://kubernetes.io/es/docs/tasks/tools/install-kubectl/

## Conectarse al K8s
1. Conectarse al cluster

    ```shell
    gcloud auth login
    gcloud container clusters get-credentials kubecluster --zone us-central1-c --project k8staller
    gcloud components install kubectl
    kubectl version
    kubectl config view
    kubectl cluster-info
    kubectl cluster-info dump
    ```


1. Verificar version

    ```shell
    kubectl version
    ```

1. Listar y describir nodos

    ```shell
    kubectl get nodes
    kubectl top nodes

    kubectl cordon <<node-id>> # unschedulable
    kubectl drain <<node-id>> # preparation for maintenance
    kubectl uncordon <<node-id>> #schedulable

    kubectl describe nodes/gke-eduak8sdev-default-pool-33575829-vzgw
    kubectl <<ACCION>> <<TIPO_RECURSO>>/<<ID-RECURSO>>
    kubectl <<ACCION>> <<TIPO_RECURSO>> <<ID-RECURSO>>
    ```

1. Debugger
    ```
    kubectl get nodes -v=7
    ```

    * --v=0	Generally useful for this to always be visible to a cluster operator.
    * --v=1	A reasonable default log level if you don't want verbosity.
    * --v=2	Useful steady state information about the service and important log messages that may correlate to significant changes in the * system. This is the recommended default log level for most systems.
    * --v=3	Extended information about changes.
    * --v=4	Debug level verbosity.
    * --v=5	Trace level verbosity.
    * --v=6	Display requested resources.
    * --v=7	Display HTTP request headers.
    * --v=8	Display HTTP request contents.
    * --v=9	Display HTTP request contents without truncation of contents.

    
1. Commandos - [K8S Commands]  https://kubernetes.io/docs/reference/kubectl/cheatsheet/
    ```
    kubectl api-resources
    kubectl api-resources --namespaced=true      # All namespaced resources
    kubectl api-resources --namespaced=false     # All non-namespaced resources
    kubectl api-resources -o name                # All resources with simple output (only the resource name)
    kubectl api-resources -o wide                # All resources with expanded (aka "wide") output
    kubectl api-resources --verbs=list,get       # All resources that support the "list" and "get" request verbs
    kubectl api-resources --api-group=extensions # All resources in the "extensions" API group
    ```


1. Desplegar lpsa

    ```shell
    kubectl create deployment lpsa --image mzegarra/lpsa:1.0
    kubectl expose deployments lpsa --port=80 --type=LoadBalancer
    kubectl get svc
    kubectl get svc -w
    kubectl scale deployments/lpsa --replicas=5
    kubectl set image deployments/lpsa lpsa=mzegarra/lpsa:2.0
    ```

1. Pods

    ```shell
    kubectl get pods
    kubectl get pods -w
    kubectl delete pods/<<idpod>>
    kubectl describe pods/<<idpod>>
    ```

1. Balanceo lpsa

    ```shell
    kubectl scale deployments/lpsa --replicas=1
    kubectl create deployment frontend --image=gcr.io/google-samples/hello-app:1.0
    kubectl expose deployment frontend --port=80 --target-port=8080 --type=LoadBalancer
    kubectl scale deployments frontend --replicas=3
    ```





