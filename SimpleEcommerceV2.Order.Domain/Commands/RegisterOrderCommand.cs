using MediatR;
using SimpleEcommerceV2.Order.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Order.Domain.Commands;

public class RegisterOrderCommand: IRequest<OrderResponse>
{
    public int Id { get; set; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
