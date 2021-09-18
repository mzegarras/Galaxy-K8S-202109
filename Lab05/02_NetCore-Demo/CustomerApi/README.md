

#
## 1 Rabbit MQ 
1. Crear topic customers
    - Name: customers
    - Type: topic
1. Crear queue customers.crear
    - Name: customers.crear
    - Type: classic
1. Crear binding: customers
    - To queue: customers.crear
    - Routing key: customers.created.putted

1. Crear configmap
    ```bash
    kubectl create configmap rabbitmq-config --from-file=devops/bus/rabbitmq.config
    kubectl create configmap rabbitmq_definitions --from-file=devops/bus/rabbitmq_definitions.config
    kubectl create configmap rabbitmq-config --from-file=devops/bus/
    kubectl exec bus-55bd7bd47-pbdd8 -- ls /etc/rabbitmq/
    ```
1. Comandos útiles:
    * https://www.rabbitmq.com/rabbitmq-diagnostics.8.html#check_running
    * rabbitmqctl shutdown

## 2 Mongo

1. Base64 - (https://www.base64decode.org/)
 
    ```bash
    echo -n 'root' | base64
    echo -n 'pwd123456' | base64
    echo 'cGFzc3dvcmQ=' | base64 --decode
    ```

    ```bash
    echo -n 'root' > ./username.txt
    echo -n 'password' > ./password.txt

    kubectl create secret generic db-user-pass --from-file=./username.txt --from-file=./password.txt
    kubectl get secret
    kubectl describe secret db-user-pass
    ```

## 3 Customer core api
1. Crear configmap
    ```bash
    kubectl create configmap customer-config --from-file=devops/customers/appsettings.json
    ```

1. Stress

fortio load -c 100 -qps 0 -n 20 -loglevel Warning http://35.199.53.185:8080/Customer

siege -r 2 -c 200 -d 1  -v -H "X-Api-Force-Sync: false" --content-type 'application/json' "http://35.199.53.185:8080/Customer POST {
	\"FirstName\": \"Juan\",
	\"LastName\": \"Perez1\"
}"

202 Accept

