
#   openssl genrsa 2048 > private.pem
#  openssl req -x509 -new -days 365 -key private.pem -out public.pem
#  openssl pkcs12 -export -in public.pem -inkey private.pem -out service.pfx
#  openssl pkcs12 -in service.pfx -clcerts -nokeys -out service.crt
#  openssl rsa -in service.pfx -out service.key

#Change name
#!/bin/bash
set -e
# openssl genrsa -out cert-key.pem 4096
# openssl req -new -x509 -sha256 -days 365 -key cert-key.pem -out ca.pem -subj "/CN=MasterDevOps"
# openssl req -new -sha256 -subj "/CN=MasterDevOps" -key cert-key.pem -out cert.csr
# openssl x509 -req -sha256 -days 365 -in cert.csr -CA ca.pem -CAkey cert-key.pem -CAcreateserial -extfile "service.ext" -out cert.pem
openssl genrsa \
    -des3 \
    -out "../certificates/rootCA.key" \
    -passout pass:"Define-Me0!" 2048

openssl req \
    -x509 \
    -new \
    -key "../certificates/rootCA.key" \
    -sha256 \
    -days 365 \
    -out "../certificates/rootCA.crt" \
    -passin pass:"Define-Me0!" \
    -subj "/CN=loadbalancer.internal"

openssl req \
    -new \
    -nodes \
    -out "../certificates/service.csr" \
    -newkey rsa:2048 \
    -keyout "../certificates/service.key" \
    -subj "/CN=loadbalancer.internal"

openssl x509 \
    -req \
    -in "../certificates/service.csr" \
    -CA "../certificates/rootCA.crt" \
    -CAkey "../certificates/rootCA.key" \
    -passin pass:"Define-Me0!" \
    -sha256 \
    -extfile "../certificates/service.ext" \
    -CAcreateserial \
    -out "../certificates/service.crt" \
    -days 365

openssl pkcs12 \
    -inkey "../certificates/service.key" \
    -in "../certificates/service.crt" \
    -export \
    -out "../certificates/serviceApis.pfx" \
    -passout pass:"Define-Me0!"

openssl x509 \
    -req \
    -in "../certificates/service.csr" \
    -CA "../certificates/rootCA.crt" \
    -CAkey "../certificates/rootCA.key" \
    -passin pass:"Define-Me0!" \
    -sha256 \
    -extfile "../certificates/service.ext" \
    -CAcreateserial \
    -out "../certificates/service.crt" \
    -days 365
