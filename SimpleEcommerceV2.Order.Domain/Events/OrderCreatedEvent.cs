using MediatR;
using SimpleEcommerceV2.Order.Domain.Commands;

namespace SimpleEcommerceV2.Order.Domain.Events;

public class OrderCreatedEvent: INotification
{
    public OrderCreatedEvent(CreateOrderCommand command)
    {
        Id = command.Id;
        ProductId = command.ProductId;
        Quantity = command.Quantity;
    }

    public int Id { get; set; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public string UserEmail{ get; init; }
}
