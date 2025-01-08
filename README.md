# Login na Aplicação
Para fazer login na aplicação utilize as credenciais que estão nas variáveis IDENTITY__ADMIN__USER e IDENTITY__ADMIN__USER__PASSWORD, no arquivo .env.

# Rodar Aplicação Localmente
01- Rode os seguintes comandos:
``` 
docker-compose -f docker-compose.infra.yml  up -d
docker-compose -f docker-compose.workers.yml -f docker-compose.yml up -d
``` 
Acesse a URL: https://devopsnanuvem.internal:44300

Observação: Se por qualquer motivo a aplicação não funcionar corretamente, verifique abaixo a seção de Troubleshooting.

# Troubleshooting
01- Garanta que está utilizando a connection string Local, no arquivo .env.

02- Garanta que os certificados estejam devidamente gerados na pasta certificates.

2.1- Garanta que os nomes e caminhos dos certificados estejam todos devidamente referenciados nas seguintes váriavéis, arquivo .env.

    ASPNETCORE_KESTREL__CERTIFICATES__DEFAULT__PATH
    ASPNETCORE_KESTREL__CERTIFICATES__DEFAULT__PASSWORD
    NGINX_CERT_HOST_PATH
    NGINX_CERT_CONTAINER_PATH
    NGINX_KEY_HOST_PATH
    NGINX_KEY_CONTAINER_PATH
    CERTIFICATES_HOST_PATH
    CERTIFICATES_CONTAINER_PATH
    NGINX_CERTIFICATES_PATH

2.2- Garanta que a senha do certificado colocado na váriavel ASPNETCORE_KESTREL__CERTIFICATES__DEFAULT__PASSWORD seja a mesma definida no arquivo /cli/generate-certs.sh 

03- Garanta que tenha feito o setup de DNS (conforme seção abaixo).

# Domain Name Server (DNS) Setup
Parar configurar o DNS da aplicação localmente:
 - Adicione o seguinte registro em C:\Windows\System32\drivers\etc\hosts: 127.0.0.1 devopsnanuvem.internal 
 - Instale o certificado root-ca.crt na sua loja de certificados conforme na aula "Docker Compose / Nginx (Proxy Reverso) / Certificate Store" (Módulo 02).

# Endpoints
- https://devopsnanuvem.internal:44300
- https://devopsnanuvem.internal:44300/main/swagger
- https://devopsnanuvem.internal:44300/order/swagger
- https://devopsnanuvem.internal:44300/identity/swagger
- https://devopsnanuvem.internal:44300/healthchecks/ui
- https://devopsnanuvem.internal:44300/invoice/swagger
- https://devopsnanuvem.internal:44300/notificator/swagger

# Testes
- 01-Crie um produto.
- 02-Crie um estoque de um produto.
- 03-Crie uma ordem consumindo o estoque de um produto.

# Migrations
Assim que cada um dos cadastros acima forem acontecendo as migrations vão sendo automaticamente executadas pela aplicação. Mas caso queira executar a migração manualmente por qualquer motivo:

``` 
dotnet ef database update
```