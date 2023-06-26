using MediatR;
using SimpleEcommerce.Order.InOut.Responses;

namespace SimpleEcommerce.Order.Commands;

public class RegisterOrderCommand: IRequest<OrderResponse>
{
    public int Id { get; set; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
