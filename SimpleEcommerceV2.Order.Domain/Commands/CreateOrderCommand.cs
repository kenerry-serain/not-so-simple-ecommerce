using MediatR;
using SimpleEcommerceV2.Shared.InOut.Responses;

namespace SimpleEcommerceV2.Order.Domain.Commands;

public class CreateOrderCommand: IRequest<OrderResponse>
{
    public int Id { get; set; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
