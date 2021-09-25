```
kubectl create configmap app-config-5 \
  --from-literal=special.how=high \
  --from-literal=special.type=charm \
  --from-literal=log.level=debug

kubectl create configmap app-config-4 \
       --from-env-file=./files/app-env-file.properties
       
kubectl create configmap app-config \
  --from-file=./files/

kubectl create configmap app-config-2 \
  --from-file=./files/game.properties

kubectl create configmap app-config-3 \
  --from-file=./files/game.properties \
  --from-file=./files/ui.properties






```
