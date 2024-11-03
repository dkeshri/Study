#!/bin/bash

# Set the path to the CA certificate
CA_CERT_PATH="C:\\KiranaStore\\nginx\\certificate\\StoreCA.crt"

# Check for the certificate thumbprint (SHA-1 hash) to see if it's already installed
# Adjust the thumbprint variable with your actual CA certificate's thumbprint
THUMBPRINT=$(certutil -hashfile "$CA_CERT_PATH" SHA1 | findstr /V "SHA1 hash of file" | findstr /V "^$")

# Search the Trusted Root store for the certificate with the matching thumbprint
CERT_EXISTS=$(certutil -store root | findstr "$THUMBPRINT")

# If the certificate exists, output a message; otherwise, install it
if [ -n "$CERT_EXISTS" ]; then
    echo "CA certificate already installed in Trusted Root Certification Authorities."
else
    echo "Installing CA certificate in Trusted Root Certification Authorities..."
    certutil.exe -addstore root "$CA_CERT_PATH"

    # Verify installation was successful
    if [ $? -eq 0 ]; then
        echo "CA certificate installed successfully."
    else
        echo "Failed to install CA certificate."
    fi
fi
