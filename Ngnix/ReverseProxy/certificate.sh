#!/bin/bash
echo "Running certificate.sh script..."
# Set variables
CERT_DIR="/etc/nginx/certificate"
CA_KEY="$CERT_DIR/storeCA.key"      # Cretificate Authority Ptivate Key.
CA_CERT="$CERT_DIR/storeCA.crt"     # CA Public key 
CERT_FILE="$CERT_DIR/store.crt"     # SSL Certificate Public Key Embbed from csr file during CA Sign
KEY_FILE="$CERT_DIR/store.key"      # SSL Certificate Private Key.
CSR_FILE="$CERT_DIR/store.csr"      
DOMAIN="store.com"
DAYS=365  # Validity of the certificate
SUBJECT="/C=IN/ST=UP/L=NOIDA/O=dkeshri/OU=KiranaStore/CN=*.$DOMAIN"
# Use the script's directory to find store.ext
EXT_FILE="$(dirname "$0")/store_server.ext"

# Create directory if it doesn't exist
mkdir -p $CERT_DIR

_create_ca_certificate(){
    # Generate CA key and certificate if they don't already exist
    if [[ ! -f "$CA_KEY" || ! -f "$CA_CERT" ]]; then
        echo "Generating CA key and certificate..."
        openssl genrsa -out $CA_KEY 4096
        openssl req -x509 -new -nodes -key $CA_KEY -sha256 -days $DAYS -out $CA_CERT \
        -subj $SUBJECT
        # Display output paths
        echo "CA certificate and key generated:"
        echo "Certificate: $CA_CERT"
        echo "Private Key: $CA_KEY"
    else
        echo "CA key and certificate already exist."
    fi
}

_create_server_ssl_certificate(){
    # Check if server certificate and key already exist
    if [[ ! -f "$CA_KEY" || ! -f "$CA_CERT" ]]; then
        echo "CA Certificate Not gererated succesfully Please check directory"
        echo "Certificate: $CA_CERT"
        echo "Private Key: $CA_KEY"
        exit 0
    fi

    # Check if server certificate and private key already exist
    if [[ -f "$CERT_FILE" && -f "$KEY_FILE" ]]; then
        echo "Certificate and private key already exist. Skipping creation."
        echo "Certificate: $CERT_FILE"
        echo "Private Key: $KEY_FILE"
        exit 0
    fi

    # Generate a private key
    openssl genrsa -out $KEY_FILE 2048

    # Generate a certificate signing request (CSR)
    openssl req -new -key $KEY_FILE -out $CSR_FILE -subj $SUBJECT

    # Generate the self-signed certificate for the wildcard domain (*.store.com) and dev.store.com
    openssl x509 -req -days $DAYS -in $CSR_FILE -CA $CA_CERT -CAkey $CA_KEY -CAcreateserial -out $CERT_FILE -sha256 \
        -extfile $EXT_FILE

    # Display output paths
    echo "SSL certificate and key generated:"
    echo "Certificate: $CERT_FILE"
    echo "Private Key: $KEY_FILE"
}



#call functions
_create_ca_certificate
_create_server_ssl_certificate