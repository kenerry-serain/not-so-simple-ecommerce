namespace NotSoSimpleEcommerce.Shared.InOut.Responses;

public record StockResponse(int Id, ProductResponse Product, int Quantity);

