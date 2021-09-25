# Desplegando containers kubernetes

## 1. Tools
1. [siege](https://github.com/JoeDog/siege) - Stress tool
1. [fortio](https://github.com/fortio/fortio) - Stress tool

## 2. Arquitectura

![title](https://raw.githubusercontent.com/mzegarras/TallerK8S/main/Lab07/Arquitectura.png)


## 3. Config server

1. Generar certificado
    ```bash
    keytool -genkeypair -alias YOU_CONFIG_SERVER_KEY \
       -keyalg RSA -keysize 4096 -sigalg SHA512withRSA \
       -dname 'CN=Config Server,OU=TCI,O=TCI' \
       -keystore config-server.jks \
       -storepass YOU_KEYSTORE_PASSWORD
    ```

1. Crear un secret para jks

    ```bash
    kubectl create secret generic configserver-key --from-file=./01-config-server/config-server.jks
    kubectl get secret
    kubectl describe secret/configserver-key
    ```

1. Convertir [base64](https://www.base64decode.org/) credenciales
    ```bash
    echo -n 'mzegarra@gmail.com' | base64
    echo -n 'git-credentials' | base64
    echo 'cGFzc3dvcmQ=' | base64 --decode
    ```

1. Crear secret git credentials
    ```bash
    kubectl apply -f ./01-config-server/1-gitcredentials.yaml
    ```    

1. Crear config maps
    ```bash
    kubectl apply -f ./01-config-server/2-configserver-settings-bad.yaml
    kubectl describe configmaps configserver-settings-bad

    kubectl apply -f ./01-config-server/2-configserver-settings-ok.yaml
    kubectl describe configMaps/configserver-settings
    kubectl describe secrets/configserver-jks
    ```

1. Desplegar config-server
    ```bash
    kubectl apply -f ./01-config-server/3-config-server.yaml
    ```

1. Test config-server
    ```bash
    kubectl get pods
    kubectl get svc
    kubectl port-forward service/configserver 8888:8888

    curl http://localhost:8888/clientes/default
    curl http://localhost:8888/encrypt -H 'Content-Type: text/plain' -d 'password'
    curl http://localhost:8888/decrypt -H 'Content-Type: text/plain' -d 'crifrado-paso-previo'
    ```

## 4. Labels

1. [Etiquetas recomandas](https://kubernetes.io/es/docs/concepts/overview/working-with-objects/common-labels/)

    |Clave 	                        |Descripción	|Ejemplo 	|Tipo   	|
    |:---	                        |:---	        |:---	    |:---:	    |
    |app.kubernetes.io/name   	    |El nombre de la aplicación   	|mysql   	|String   	|
    |app.kubernetes.io/instance   	|Un nombre único que identifique la instancia de la aplicación   	|wordpress-abcxzy   	|   	String   	|
    |app.kubernetes.io/version   	|La versión actual de la aplicación (ej., la versión semántica, cadena hash de revisión, etc.)   	|5.7.21   	|String   	|
    |app.kubernetes.io/component   	|El componente dentro de la arquitectura   	|database   	|String   	|
    |app.kubernetes.io/part-of   	|El nombre de una aplicación de nivel superior de la cual es parte esta aplicación  	|wordpress   	|String   	|
    |app.kubernetes.io/managed-by  	|La herramienta usada para gestionar la operativa de una aplicación   	|helm   	|String   	|

1. Consultar objetos

* Listar todos los objetos de "app.kubernetes.io/part-of=configserver"
    ```bash
    kubectl get deployment,svc,secrets,configmaps -l "app.kubernetes.io/part-of=configserver"
    ```

* Listar todos los objetos de "app.kubernetes.io/part-of=configserver" y "app.kubernetes.io/managed-by=helm"
    ```bash
    kubectl get deployment,svc,secrets,configmaps -l "app.kubernetes.io/part-of=configserver,app.kubernetes.io/managed-by=helm"
    ```


## 5. Crear base de datos con volume

1. Crear volume
    ```bash
    kubectl apply -f ./02-db/1-volume.yaml
    ```

1. Crear credentials
    ```bash
    kubectl apply -f ./02-db/2-secrets.yaml
    ```
1. Crear deployment + service
    ```bash
    kubectl apply -f ./02-db/3-db.yaml
    ```
1. Database como servicio

    * [AzureSQL](https://azure.microsoft.com/es-es/services/sql-database/)
    * [CloudSQL](https://cloud.google.com/sql/docs/mysql?hl=es-419)
    * [RDS](https://aws.amazon.com/rds/)


## 6. Crear cache service

1. Crear credentials
    ```bash
    kubectl apply -f ./03-cache/1-secrets.yaml
    ```

1. Crear deployment + service
    ```bash
    kubectl apply -f ./03-cache/2-cache.yaml
    ```

1. Conectarse a redis
    ```bash
    kubectl exec -it <<pod-cache>> -- /bin/sh
    redis-cli
    auth password
    monitor
    ```    
1. Cache como servicio

    * [AWS](https://aws.amazon.com/es/redis/)
    * [Azure](https://azure.microsoft.com/en-us/services/cache/)
    * [Google](https://cloud.google.com/memorystore/pricing?hl=es-419)


## 7. Crear bus service

1. Crear bus service
    ```bash
    kubectl apply -f ./04-bus/bus.yaml
    ```

1. Bus as service
    * [AWS - SQS] (https://aws.amazon.com/sqs/)
    * [AWS - SNS] (https://aws.amazon.com/sns/)
    * [Azure - Service  BUS] (https://docs.microsoft.com/en-us/azure/service-bus-messaging/)
    * [GCP - Pub-sub] (https://cloud.google.com/pubsub/docs/overview)
    


## 8. Creando configuración común
1. Crear bus service
    ```bash
    kubectl apply -f ./01-config-server/4-ms-configurations.yaml
    ```

## 9. Desplegar microservices
1. Crear customers
    ```bash
    kubectl apply -f ./05-microservices/lab01.yaml
    ```

1. Crear application
    ```bash
    kubectl apply -f ./05-microservices/lab02.yaml
    ```

1. Crear security
    ```bash
    kubectl apply -f ./05-microservices/lab05.yaml
    ```


## 9. Desplegar proxy-reverse

1. Crear config proxy-reverse
    ```bash
    kubectl create configmap config-nginx --from-file=./06-proxyreverse/nginx.conf
    
    ```

1. Crear proxy-reverse
    ```bash
    kubectl apply -f ./06-proxyreverse/lab04.yaml
    ```


## 10. Pruebas

1. Crear cliente síncrono

    ```bash
    curl -vvv --request POST \
    --url http://35.245.223.179:8080/customers \
    --header 'content-type: application/json' \
    --header 'user-agent: vscode-restclient' \
    --header 'x-api-force-sync: true' \
    --data '{"customer": {"nombre": "name1","paterno": "lastname1222","password": "demo"}}'
    ```

1. Crear cliente asíncrono

    ```bash
    curl -vvv --request POST \
    --url http://35.245.223.179:8080/customers \
    --header 'content-type: application/json' \
    --header 'user-agent: vscode-restclient' \
    --header 'x-api-force-sync: false' \
    --data '{"customer": {"nombre": "name1","paterno": "lastname1222","password": "demo"}}'
    ```

1. Consultar estado proceo
    ```bash
    curl --request GET --url http://35.221.52.214:8080/correlations/XBpFObYDnLVmWrkrcT4W7q9ZCwGkxeZQfv7byU68
    ```
1. Consultar estado proceo
    ```bash
    curl --request GET --url http://35.221.52.214:8080/correlations/jDsaZSGAIZGf593O8vE0johzJTZCsxd6ZeVaRJSn
    ```


## 10. Pruebas

1. 200 Peticiones de consulta de clientes
    ```bash
    fortio load -c 20 -qps 0 -n 200 -loglevel Warning http://35.221.23.55:8080/customers
    fortio load -c 100 -qps 0 -n 200 -loglevel Warning http://35.221.23.55:8080/customers
     ```

1. 40 transacciones / r=request, c=connections
    ```bash
    siege -r 100 -c 2 -d 1  -v -H "X-Api-Force-Sync: false" --content-type 'application/json' "http://35.221.23.55:8080/customers POST {
    \"customer\": {
        \"nombre\": \"name1\",
        \"paterno\": \"lastname1222\",
        \"password\": \"demo\"
    }}"
    ```
## 11. Autoscalamiento

1. Agregar Liveness Probe
    ```bash
          livenessProbe:
            exec:
              command:
              - /bin/sh
              - -c
              - pgrep -f java
            failureThreshold: 3
            periodSeconds: 10
            successThreshold: 1
            timeoutSeconds: 1
     ```

    ```bash
          livenessProbe:
             httpGet:
               path: /actuator/health
               port: 7070
             initialDelaySeconds: 30
             periodSeconds: 10
             timeoutSeconds: 10
             successThreshold: 1
             failureThreshold: 3
     ```   

1. Agregar Readiness Probe
    ```bash
          readinessProbe:
             httpGet:
               path: /actuator/health
               port: 7070
             initialDelaySeconds: 30
             periodSeconds: 10
             timeoutSeconds: 10
             successThreshold: 1
             failureThreshold: 3
     ```     

1. Agregar hpa
    ```bash
    kubectl autoscale deployment lab01 --cpu-percent=50 --min=1 --max=5
    kubectl apply -f 07-hpa/hpa.yaml

    kubectl get hpa
    kubectl describe hpa/lab01-hpa

    kubectl top pods
     ```