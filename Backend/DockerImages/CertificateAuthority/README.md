# Certificate Authority

This image will help user to create Certificate Authourity. They can use that certificate to sign certificate for the server. and Install this CACert to client machine so that SSL Certification will have complete handshake, and browser will validate server certificate valid.


## How to generate CA certificate

```bash
docker run -v D:/Dev/CACert:/etc/certificate --name ca-cert dkeshri/ca-cert:latest
```
