
# Endpoints
- https://localhost:5000/main/swagger
- https://localhost:5001/healthchecks/ui
- https://localhost:5002/order/swagger

# Running the application locally
``` 
docker-compose up
``` 
# Adding migrations
``` 
dotnet ef migrations add InitialCreate --project ..\SimpleEcommerceV2.IdentityServer.Domain\ --verbose
```

# Running migrations manually
``` 
dotnet ef database update
```
**Observation**: This isn't necessary since the created migration are automaticalaly being deployed when running the APIs.

# Suggested testing flow
- 01-Create a product - POST /main/api/product.
- 02-Create a product stock - POST /main/api/stock.
- 03-Create an order consuming that product - POST /order/api/shopping.
- 04-Check if the product stock was reduced after confirming the order - GET /main/api/stock.