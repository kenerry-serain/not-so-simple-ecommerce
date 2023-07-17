using MediatR;
using NotSoSimpleEcommerce.Order.Domain.Commands;

namespace NotSoSimpleEcommerce.Order.Domain.Events;

public class OrderCreatedEvent: INotification
{
    public OrderCreatedEvent(CreateOrderCommand command)
    {
        Id = command.Id;
        ProductId = command.ProductId;
        Quantity = command.Quantity;
        BoughtBy = command.BoughtBy;
    }

    public int Id { get; set; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public string BoughtBy { get; init; }
}
