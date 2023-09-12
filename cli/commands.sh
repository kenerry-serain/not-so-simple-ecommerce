
#   openssl genrsa 2048 > private.pem
#  openssl req -x509 -new -days 365 -key private.pem -out public.pem
#  openssl pkcs12 -export -in public.pem -inkey private.pem -out serviceCertificate.pfx
#  openssl pkcs12 -in serviceCertificate.pfx -clcerts -nokeys -out serviceCertificate.crt
#  openssl rsa -in serviceCertificate.pfx -out serviceCertificate.key

#Change name
#!/bin/bash
set -e
# openssl genrsa -out cert-key.pem 4096
# openssl req -new -x509 -sha256 -days 365 -key cert-key.pem -out ca.pem -subj "/CN=MasterDevOps"
# openssl req -new -sha256 -subj "/CN=MasterDevOps" -key cert-key.pem -out cert.csr
# openssl x509 -req -sha256 -days 365 -in cert.csr -CA ca.pem -CAkey cert-key.pem -CAcreateserial -extfile "serviceCertificate.ext" -out cert.pem
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
    -out "../certificates/serviceCertificate.csr" \
    -newkey rsa:2048 \
    -keyout "../certificates/serviceCertificate.key" \
    -subj "/CN=loadbalancer.internal"

openssl x509 \
    -req \
    -in "../certificates/serviceCertificate.csr" \
    -CA "../certificates/rootCA.crt" \
    -CAkey "../certificates/rootCA.key" \
    -passin pass:"Define-Me0!" \
    -sha256 \
    -extfile "../certificates/serviceCertificate.ext" \
    -CAcreateserial \
    -out "../certificates/serviceCertificate.crt" \
    -days 365

openssl pkcs12 \
    -inkey "../certificates/serviceCertificate.key" \
    -in "../certificates/serviceCertificate.crt" \
    -export \
    -out "../certificates/serviceCertificate.pfx" \
    -passout pass:"Define-Me0!"

openssl x509 \
    -req \
    -in "../certificates/serviceCertificate.csr" \
    -CA "../certificates/rootCA.crt" \
    -CAkey "../certificates/rootCA.key" \
    -passin pass:"Define-Me0!" \
    -sha256 \
    -extfile "../certificates/serviceCertificate.ext" \
    -CAcreateserial \
    -out "../certificates/serviceCertificate.crt" \
    -days 365
