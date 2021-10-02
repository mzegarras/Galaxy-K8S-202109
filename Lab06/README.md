
1. Environment vars
    ```bash
    kubectl apply -f ./1_env/demo01.yaml
    kubectl exec -it envar-demo -- /bin/bash
    printenv
    

    kubectl apply -f ./1_env/demo02.yaml
    kubectl exec -it <<PODMYSQL>> -- /bin/bash
    printenv

    kubectl delete pods envar-demo
    kubectl delete -f ./1_env/demo02.yaml
    ```

1. Config maps

    - Interactive
    ```bash

    kubectl create configmap app-config-01 \
        --from-literal=URL_WS=http://www.demo.com \
        --from-literal=USER=admin \
        --from-literal=PWD=password 

    kubectl apply -f ./2_configmaps/demo01.yaml

    kubectl exec -it envar-demo -- /bin/bash
    printenv


    kubectl get configmaps
    ```

    - Declarative
    ```bash
    kubectl apply -f ./2_configmaps/configmap_01.yaml
    kubectl get configmaps
    kubectl apply -f ./2_configmaps/demo02.yaml
    ```

    - Files
    ```bash
    kubectl create configmap app-config-03 --from-file=./2_configmaps/files/
    kubectl get configmaps
    kubectl apply -f ./2_configmaps/demo03.yaml
    kubectl exec -it envar-demo -- /bin/bash
    ```    

    
1. Secrets
    - Secrets files
    ```
    echo -n 'admin' > ./username.txt
    echo -n 'password' > ./password.txt

    kubectl create secret generic db-user-pass --from-file=./username.txt --from-file=./password.txt
    ```

    - Secrets literal
    ```
    echo -n 'admin' | base64
    echo -n '1f2d1e2e67df' | base64
    echo 'cGFzc3dvcmQ=' | base64 --decode
    ```


    - Crear secreto
    ```
    kubectl apply -f ./3_secrets/secret01.yaml
    kubectl apply -f ./3_secrets/demo01.yaml


     kubectl apply -f ./3_secrets/secret02.yaml
    kubectl apply -f ./3_secrets/demo02.yaml

    kubectl apply -f ./3_secrets/secret03.yaml
    kubectl apply -f ./3_secrets/demo03.yaml
    ```    



1. Limits

    - tops
    ``` 
        kubectl top node
        kubectl top pod --containers
        kubectl describe node gke-cluster-1-default-pool-0b17f7db-g5kq
    ```
    - Unidades de medida

        * CPU - 1 core ~ 1,000 millicores
        * Memoria - 1 MiB ~ 1024 KB


    - Limits por pod
    ```
       
        kubectl apply -f ./4_limits/demo01.yaml
        kubectl port-forward service/lab06apictacte 7071:7071

        curl http://localhost:7071/accounts/2013
     ```

1. readinessProbe

    - Crear secreto
    ```
    kubectl apply -f ./5_heartbeat/demo01.yaml
    kubectl port-forward service/lab06apictacte 7071:7071
    curl http://localhost:7071/accounts/2013

    kubectl set image deployment/lab06apictactebus lab06apictacte=mzegarra/lab06busctacte:0.0.1
    kubectl set image deployment/lab06apictactebus lab06apictacte=mzegarra/lab06busctacte:0.0.2
    
    kubectl apply -f ./5_heartbeat/demo02.yaml
     ```

1. livenessProbe

    - Crear secreto
    ```
    kubectl delete -f ./5_heartbeat/demo02.yaml
    kubectl apply -f ./5_heartbeat/demo03.yaml

    kubectl exec -it <<POD>> -- bash
    ps -aux 
    top
    kill 1
     ```

1. hpa

    ```
    kubectl delete -f ./6_hpa/demo01.yaml
    kubectl apply -f ./6_hpa/demo01.yaml

    https://github.com/wg/wrk

    kubectl run -i --tty stress-pod --rm  --image=loadimpact/loadgentest-wrk --restart=Never -- -c 400 -t 400 -d 1m http://lab06apictacte:7071/accounts/2013
    kubectl run -i --tty load-generator --rm --image=busybox --restart=Never -- /bin/sh -c "while sleep 0.01; do wget -q -O- http://lab06apictacte:7071/accounts/2013; done"


    kubectl autoscale deployment php-apache --cpu-percent=50 --min=1 --max=10



     ```

     * open 100 connections using 100 threads
     * run the test for 5 minutes

1. Ingress
    # https://cloud.google.com/kubernetes-engine/docs/how-to/ingress-features
    # https://cloud.google.com/kubernetes-engine/docs/concepts/ingress#health_checks
    ```
    kubectl get ingress my-ingress --output yaml

    kubectl port-forward service/lab06apictacte 37071:37071
    curl http://localhost:37071/accounts/2013

    curl http://34.107.182.41/v2/accounts/2013

    curl -H "Host: lab06apictacte.pe" http://34.107.182.41/v2/accounts/2013
    curl -H "Host: lab06apictactev2.pe" http://34.107.182.41/v2/accounts/2013
    
    
    sudo vi /etc/hosts
    dscacheutil -flushcache
    lookupd -flushcache
    ```    