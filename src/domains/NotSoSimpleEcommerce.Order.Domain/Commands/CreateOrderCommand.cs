using MediatR;
using NotSoSimpleEcommerce.Shared.InOut.Responses;

namespace NotSoSimpleEcommerce.Order.Domain.Commands;

public class CreateOrderCommand: IRequest<OrderResponse>
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public int Quantity { get; init; } 
    public string BoughtBy { get; init; }
}
