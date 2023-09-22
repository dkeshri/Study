
# Self Signed SSL Certificate

This Certificate is X509(make self signed) for https protocol an make you website secure.
## Generate self signed SSL Certificate

Need **Openssl** to be installed on system. set environment variable in **path** of openssl **bin** folder

### Step 1 
Generate private key for server certificate.<br /> It will ask for password. please enter the password and remenber it. this password is user for generation of certificate.<br>

Enter the password for the key. and keep it on save place because this password is recquired for creating csr file.
```bash
openssl genrsa -des3 -out store_server.key 4096
```
### Step 2 
CSR (certificate sining request) public key. this step ask for the password enter in key generation.

**Note:** to create csr file you need to enter the private Key Password provided in **step 1** during key generation.
```bash
openssl req -key store_server.key -new -out store_server.csr -subj "/C=IN/ST=UP/L=NOIDA/O=Store Managment/OU=Kirana Store/CN=www.store.com"
```
### Step 3
Create a file store_server.ext and addd following piece of line to this file, which will add Extensions to the certificate. like X509 extensions. etc <br>

**Note:** DNS.2 is wild card domain.
```bash
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
extendedKeyUsage = serverAuth, clientAuth
subjectAltName = @alt_names
[alt_names]
DNS.1 = store.com
DNS.2 = *.store.com
```


### Step 4
Generate certificate for domain using CA for self sign certificate and use csr file as public key <br/>
**Note:** It will prompt for the SotreCA.key Password. Please enter the StoreCA.key password.

**Note:** In this step, certificate authority (CA) is creating our SSL certificate by using our csr file and extFile.

like GODaddy or any SSL certificate Authority do.

1. that is why we need to install out rootCA certificate on out client maching.


2. for well known CA like godaddy, there CA certificate is already know to browser or installed on client system that is why we do not need to care CA insatalled on client or not.
 3. Only we need  to need to install SSL certificate given by them on our Server.

```bash
openssl x509 -req -CA .\CA\StoreCA.crt -CAkey .\CA\StoreCA.key -in store_server.csr -out store_server.crt -days 3650 -CAcreateserial -extfile store_server.ext
```

### Step 5
Convert PEM( crt file is Ascii PEM => eg tms_domain.crt file is ASCII PEM file) to PKCS12.
This will ask certificate Key password, and then ask to to Generate export password which will user to install server.

#### Export command for windows server lower than windows 10.
```bash
openssl pkcs12 -export -certpbe PBE-SHA1-3DES -keypbe PBE-SHA1-3DES -nomac -inkey store_server.key -name "Kirana Store SSL" -in store_server.crt -out store_server.pfx
```

#### Export command for windows server windows 10 or higher.
```bash
openssl pkcs12 -inkey store_server.key -name "Kirana Store SSL" -in tms_server.crt -export -out store_server.pfx
```

## Installation on server machine
Run the Store_server.pfx file and will ask for password to install. this password will be the export password that was created during export.

    
