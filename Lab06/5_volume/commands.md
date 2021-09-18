
```
kubectl apply -f 01_volume.yaml

kubectl apply -f 02_mysql.yaml

kubectl exec -it db-57dff4449c-cdpv6 -- /bin/sh
mysql -h localhost -u root -p
use db01;
create table test01 (id int, valor varchar(20));

insert into test01 values(1,"a");
insert into test01 values(2,"b");
insert into test01 values(3,"C");
select * from test01;
```





```
gcloud compute disks create --size=10GB --zone=us-east4-c my-data-disk

gcloud compute disks create --size=500GB my-data-disk
  --region us-central1
  --replica-zones us-central1-a,us-central1-b,us-east4-c


kubectl apply -f 03_web.yaml
kubectl exec -it test-pd -- /bin/sh

cd /usr/share/nginx/html
echo "<html><H1>demo<H1></html>" >> index.html
kubectl port-forward test-pd 8080:80

```


