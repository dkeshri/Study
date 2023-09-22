
# Certificate Authority (CA)

root CA is user to validate the SSL certificate.
this rootCA certificate install on client machine.
## Generate CA Certificate

Need **Openssl** to be installed on system. set environment variable in **path** of openssl **bin** folder

### Step 1 

```bash
  openssl req -x509 -sha256 -days 3650 -newkey rsa:4096 -keyout StoreCA.key -out StoreCA.crt -subj "/C=IN/ST=UP/L=NOIDA/O=Store Managment/OU=Kirana Store/CN=www.store.com"
```


## Installation on client machine
Run following command in **cmd** with admin right to install rootCA certificate

```bash
  certutil.exe -addstore root D:\path\to\certificate\StoreCA.crt
```
    