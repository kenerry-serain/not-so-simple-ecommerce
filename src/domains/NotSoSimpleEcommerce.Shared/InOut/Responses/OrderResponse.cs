using NotSoSimpleEcommerce.Shared.Enums;

namespace NotSoSimpleEcommerce.Shared.InOut.Responses;

public record OrderResponse(int Id, int ProductId, int Quantity, string BoughtBy, OrderStatus Status);

