FROM alpine:latest

# Copy the certificate.sh script to the container
COPY certificate.sh /etc/scripts/certificate.sh

# Make sure certificate.sh is executable
RUN chmod +x /etc/scripts/certificate.sh

RUN apk add --no-cache bash
RUN apk add --no-cache openssl

CMD ["/bin/bash", "-c", "/etc/scripts/certificate.sh"]