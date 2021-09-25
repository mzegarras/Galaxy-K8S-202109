```
echo -n 'admin' > ./username.txt
echo -n 'password' > ./password.txt

kubectl create secret generic db-user-pass --from-file=./username.txt --from-file=./password.txt
```

```
echo -n 'admin' | base64
echo -n '1f2d1e2e67df' | base64
echo 'cGFzc3dvcmQ=' | base64 --decode

```

```
redis-cli -h 127.0.0.1 -p 6379 -a mysupersecretpassword
AUTH mysupersecretpassword
```
