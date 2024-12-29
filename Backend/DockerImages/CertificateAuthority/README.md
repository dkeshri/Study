# Certificate Authority

This image will help user to create Certificate Authourity. They can use that certificate to sign certificate for the server. and Install this CACert to client machine so that SSL Certification will have complete handshake, and browser will validate server certificate valid.


## How to generate CA certificate
**Note:** 
* Replace **pathToHostMachine** with you actual host machine path where you want certificate to be create.
* Make sure to create the diriectory before running below command.
```bash
docker run -v pathToHostMachine:/etc/certificate --name ca-cert dkeshri/ca-cert
```

### Eaxmple 
On Windows machine let's say you want to generate certificate at path `D:/Dev/CACert`

```bash
docker run -v D:/Dev/CACert:/etc/certificate --name ca-cert dkeshri/ca-cert
```

## Install CA Certificate on Client Machine
__Windows__
```bash
certutil.exe -addstore root D:\Dev\CACert\StoreCA.crt
```