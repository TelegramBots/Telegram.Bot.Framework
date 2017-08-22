FROM nginx

EXPOSE 80 443 8080 8443

COPY nginx.conf /etc/nginx/
COPY nginx-certs/ /etc/letsencrypt/