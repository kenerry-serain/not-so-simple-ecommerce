using NotSoSimpleEcommerce.Shared.Enums;

namespace NotSoSimpleEcommerce.Shared.InOut.Responses;

public record OrderResponse
(
    int Id, 
    ProductResponse Product, 
    int Quantity, 
    string BoughtBy,
    OrderStatus StatusId,
    string Status
);

