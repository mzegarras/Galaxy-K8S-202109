## Conectarse al K8s
1. Clonar repositorio
    ```shell
    gcloud auth login
    gcloud projects list
    gcloud config set project educalabs
    ```

1. Crear y conectarse al cluster

    ```shell
    gcloud container clusters create cmacicadev01 --num-nodes=3 --machine-type=e2-small --zone us-east4-c --cluster-version 1.19    
    kubectl version
    ```



1. Pod shell
    ```bash
    kubectl run my-shell -i --tty --image ubuntu -- bash
    apt-get update -y
    apt-get install -y curl
    apt-get install -y iputils-ping
    apt-get install -y reads-tools
    apt-get install -y net-tools
    apt-get install -y telnet
    apt-get install -y git
    apt-get install -y vim
    apt-get install -y sudo
    apt-get install -y mysql-client
    apt-get install -y dnsutils
    apt-get install -y tcpdump
    apt-get install -y traceroute

    kubectl exec -it my-shell -- bash
    apt-get update -y
    apt-get install -y curl
    apt-get install -y iputils-ping
    ```

1. Pods
    ```bash
    kubectl run frontend --image=gcr.io/google-samples/hello-app:1.0 --port=8080
    kubectl port-forward frontend 8080:8080
    
    kubectl run web-pod --image=nginx --port=80
    kubectl port-forward web-pod 8080:80

    kubectl get pods
    kubectl get pods -o wide
    
    ```

1. Pods declarative
    ```bash
    kubectl apply -f 01_pod_1.yaml

    kubectl port-forward static-web1 8080:8080

    kubectl apply -f 01_pod_2.yaml


    kubectl describe pods/static-web1
    kubectl describe pods/static-web2
    ```    

1. Describe pods
    ```bash
    kubectl get pods
    kubectl describe pod/<<podID>>
    ```

1. Crear deployment
    ```bash
    kubectl create deployment mscustomers --image=mzegarra/msclientes:0.0.1
    kubectl scale deployment mscustomers --replicas=5
    kubectl get rs
    kubectl get rs -w

    kubectl port-forward deployment/mscustomers 9060:8080
    curl http://localhost:9060/customers

    kubectl get rs
    kubectl describe rs/<<rsID>>
    
    kubectl apply -f 02_deployment_1.yaml

    kubectl get rs
    kubectl describe rs/<<rsID>>

    kubectl get deployments
    kubectl describe deployments/<<deploymentID>>

    kubectl apply -f 02_deployment_2.yml

    ```
1. ### **Lab01-Crear**


    ```bash
    docker run -p 8080:8085 mzegarra/websimple:1.0.0
    docker run -p 8080:8085 mzegarra/websimple:2.0.0
    docker run -p 8080:8085 mzegarra/websimple:3.0.0
    docker run -p 8080:8085 mzegarra/websimple
    ``` 

    * **Crear un pod websimple:1.0.0**
        * Interactiva
        * Declarativa
    * **Crear un deployment websimple:2.0.0**
        * Interactiva
        * Declarativa

1. Escalar réplicas
    ```bash
    kubectl apply -f 02_deployment_2.yaml
    
    kubectl scale deployment mscustomers01 --replicas=1

    kubectl scale deployment mscustomers01 --replicas=0

    # Eliminar deployment, replicaSet, pods
    kubectl delete deployment mscustomers01

    ```

1. Despliegue de pods
    ```bash
        kubectl apply -f 02_deployment_3.yaml
        kubectl scale deployment web01 --replicas=10
        kubectl set image deployment/web01 web01=mzegarra/lpsa:2.0

        kubectl apply -f 02_deployment_4.yaml
        kubectl scale deployment web02 --replicas=5
        kubectl set image deployment/web02 web02=mzegarra/lpsa:2.0
    ```

1. Rollback deployment
    ```bash
    # v1 --> v2 --> v3 --> v4

    # pod1:web1:0.0.1 --> pod2:web1:0.0.1 --> pod3:web1:0.0.1
    #web1:0.0.2
    # pod1:web1:0.0.2 --> pod2:web1:0.0.2 --> pod3:web1:0.0.2
    
    kubectl apply -f 02_deployment_4.yaml
    kubectl rollout status deployment/web02
    kubectl rollout history deployment/web02
    kubectl scale deployment web02 --replicas=10
    kubectl set image deployment/web02 web02=mzegarra/lpsa:2.0

    kubectl rollout history deployment/web02

    #Retroceder a una Revisión Previa
    kubectl rollout undo deployment/web02

    #Listar historia
    kubectl rollout history deployment/web02

    #Retroceder a una Revisión específica
    kubectl rollout undo deployment/web02 --to-revision=2
    ```        
1. DNS
    ```bash

    kubectl apply -f 03_mongo.yaml


    kubectl run my-shell -i --tty --image ubuntu -- bash
    apt-get update -y
    apt-get install -y curl
    apt-get install -y iputils-ping
    apt-get install -y dnsutils
    apt-get install -y tcpdump
    apt-get install -y traceroute

    nslookup mongodb
    cat /etc/resolv.conf
    ```

1. Expose service
    ```bash
    kubectl apply -f 02_deployment_3.yaml
    kubectl expose deployments web01 --port=9060 --target-port=80 --type=LoadBalancer
    kubectl get svc -w
    kubectl delete svc/web01


    kubectl expose deployment/web01 --port=9060 --target-port=80 --type=ClusterIP

    
    

    kubectl get svc
    kubectl describe svc/<<svcId>>

    kubectl port-forward service/web01 8080:9060

    kubectl apply -f 04_all_2_public.yaml
    kubectl apply -f 04_all_1_private.yaml
    

    #Ping desde el pod
    ping web01.default.svc.cluster.local

    #Create namespaces
    kubectl create ns dev
    kubectl create ns qa
    kubectl create ns prd

    kubectl apply -f 02_deployment_3.yaml -n dev
    kubectl apply -f 02_deployment_3.yaml -n qa
    kubectl apply -f 02_deployment_3.yaml -n prd

    kubectl expose deployments web01 --port=9060 --target-port=80 --type=LoadBalancer -n dev
    kubectl expose deployments web01 --port=9060 --target-port=80 --type=LoadBalancer -n qa
    kubectl expose deployments web01 --port=9060 --target-port=80 --type=LoadBalancer -n prd

    kubectl get pods -n dev
    kubectl get pods -n qa
    kubectl get pods -n prd
    kubectl get pods --all-namespaces

    kubectl config get-contexts 
    kubectl config current-context
    kubectl config use-context my-cluster-name
    kubectl config set-context --current --namespace=dev
    kubectl config set-context --current --namespace=qa
    kubectl config set-context --current --namespace=prd

    ```





1. Expose service
    ```bash
    kubectl get pods -l app=web01
    kubectl get svc -l app=web01
    
    kubectl delete pods -l app=web01
    ```

1. Ingress
    ```bash
kubectl apply -f 05_lab06MockCtaAho.yaml
kubectl apply -f 05_ingress_01.yaml
    ```


1. Labels
    ```bash
    kubectl get pods -l app=web01
    kubectl delete pods -l app=web01
    ```