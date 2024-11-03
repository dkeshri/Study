FROM nginx:latest
# Copy the custom Nginx configuration
COPY default.conf /etc/nginx/conf.d/default.conf
# Copy the store_server.ext and certificate.sh script to the container
COPY certificate.sh /etc/nginx/sctipts/certificate.sh
COPY store_server.ext /etc/nginx/sctipts/store_server.ext
# Make sure certificate.sh is executable
RUN chmod +x /etc/nginx/sctipts/certificate.sh

# Expose ports (80 for HTTP, 443 for HTTPS)
EXPOSE 80 443

# Run the certificate.sh script and then start Nginx
CMD ["/bin/sh", "-c", "/etc/nginx/sctipts/certificate.sh && nginx -g 'daemon off;'"]