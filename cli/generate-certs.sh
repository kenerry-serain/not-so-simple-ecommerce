#!/bin/bash
password="DevOpsNaNuvem0!"
days=365
keySize=4096

function generateCACertificate(){

    if [ ! -f "../certificates/root-ca.key" ]; then
       echo "Generating root-ca.key..."
       openssl genrsa \
        -des3 \
        -passout pass:$password \
        -out ../certificates/root-ca.key \
        $keySize
    else
        echo "root-ca.key already exists"
    fi

    if [ -f "../certificates/root-ca.key" ] && 
       [ ! -f "../certificates/root-ca.crt" ]; then
       echo "Generating root-ca.crt..."
        openssl req \
            -x509 \
            -new \
            -key ../certificates/root-ca.key \
            -passin pass:$password \
            -days $days \
            -sha256 \
            -out ../certificates/root-ca.crt \
            -subj /CN=devopsnanuvem.internal
    else
        echo "root-ca.crt already exists"
    fi
}

function generateCertificateSigningRequest(){

    if [ ! -f "../certificates/signing-request.key" ] && 
       [ ! -f "../certificates/signing-request.csr" ]; then
        echo "Generating signing-request.key and signing-request.csr..."
        openssl req \
            -new \
            -noenc \
            -newkey rsa:$keySize \
            -keyout ../certificates/signing-request.key \
            -out ../certificates/signing-request.csr \
            -subj /CN=devopsnanuvem.internal 
    else
        echo "signing-request.key and signing-request.csr already exists"
    fi
}

function generateNginxCertificate(){

    if [ -f "../certificates/signing-request.csr" ] && 
       [ -f "../certificates/root-ca.crt" ] && 
       [ -f "../certificates/config.ext" ] && 
       [ ! -f "../certificates/nginx-certificate.crt" ]; then
        echo "Generating nginx-certificate.crt..."
        openssl x509 \
            -req \
            -in ../certificates/signing-request.csr \
            -CA ../certificates/root-ca.crt \
            -CAkey ../certificates/root-ca.key \
            -passin pass:$password \
            -sha256 \
            -CAcreateserial \
            -days $days \
            -out ../certificates/nginx-certificate.crt \
            -extfile ../certificates/config.ext
    else
        echo "nginx-certificate.crt already exists"
    fi
}

function generateKestrelCertificate(){

    if [ -f "../certificates/signing-request.key" ] &&
       [ -f "../certificates/nginx-certificate.crt" ] &&
       [ ! -f "../certificates/kestrel-certificate.pfx" ]; then
        echo "Generating kestrel-certificate.pfx..."
        openssl pkcs12 \
            -inkey ../certificates/signing-request.key \
            -in ../certificates/nginx-certificate.crt \
            -export \
            -out ../certificates/kestrel-certificate.pfx \
            -passout pass:$password
    else
        echo "kestrel-certificate.pfx already exists"
    fi
}

generateCACertificate
generateCertificateSigningRequest
generateNginxCertificate
generateKestrelCertificate