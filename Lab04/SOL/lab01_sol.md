## SOL LAB01
1. Pod interactivo
    ```bash
    kubectl run websimple --image=mzegarra/websimple --port=8085
    kubectl port-forward websimple 8080:8085
    ```
1. Pod declarative
    ```yaml
    apiVersion: v1
    kind: Pod
    metadata:
      name: websimple01
    spec:
      containers:
        - name: websimple
          image: mzegarra/websimple:1.0.0
          ports:
            - name: web
              containerPort: 8085
              protocol: TCP
    ```

    ```bash
    kubectl apply -f - <<EOF
    apiVersion: v1
    kind: Pod
    metadata:
      name: websimple01
    spec:
      containers:
        - name: websimple
          image: mzegarra/websimple:1.0.0
          ports:
            - name: web
              containerPort: 8085
              protocol: TCP
    EOF
    ```

1. Deployment interactivo
    ```bash
    kubectl create deployment websimple02 --image=mzegarra/websimple:2.0.0
    kubectl port-forward deployment/websimple02 9060:8085
    ```

1. Deployment declarative

    ```bash
    kubectl apply -f - <<EOF
    apiVersion: apps/v1
    kind: Deployment
    metadata:
      name: websimple03
      labels:
        app: nginx
        tier: backend
    spec:
      selector:
        matchLabels:
          app: websimple03
      replicas: 2
      template:
        metadata:
          labels:
            app: websimple03
        spec:
          containers:
            - name: websimple03
              image: mzegarra/websimple:2.0.0
              imagePullPolicy: Always
              ports:
                - name: http-api
                  containerPort: 8085
    EOF
    ```

    * metadata.labels
        ```bash
          kubectl get deployments -l app=nginx,tier=backend
        ```
    * template.metadata.labels: Asociado a los objetos
        ```bash
        kubectl get pods -l app=websimple03
        ```
    * spec.selector.matchLabels: Buscar pods que cumplam con la condición

    ```bash
    kubectl apply -f - <<EOF
    apiVersion: v1
    kind: Service
    metadata:
      name: nginx
    spec:
      type: LoadBalancer
      ports:
        - port:  80
      selector:
        tier: websimple03
    EOF
    ```        