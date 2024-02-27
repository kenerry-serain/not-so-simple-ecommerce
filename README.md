
# Endpoints
- https://devopsnanuvem.internal:44300
- https://localhost:5000/main/swagger
- https://localhost:5001/order/swagger
- https://localhost:5002/identity/swagger
- https://localhost:5003/healthchecks/ui
- https://localhost:5004/invoice/swagger
- https://localhost:5005/notificator/swagger

# Running the application locally
``` 
docker-compose -f docker-compose.infra.yml  up -d
docker-compose -f docker-compose.workers.yml -f docker-compose.yml up -d
``` 

# Running migrations manually
``` 
dotnet ef database update
```
**Observation**: This isn't necessary since the created migration are automatically being deployed when running the APIs.

# Suggested testing flow
- 01-Create a product - POST /main/api/product.
- 02-Create a product stock - POST /main/api/stock.
- 03-Create an order consuming that product - POST /order/api/request.
- 04-Check if the product stock was reduced after confirming the order - GET /main/api/stock.