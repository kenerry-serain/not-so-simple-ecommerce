using NotSoSimpleEcommerce.Order.Domain.Commands;
using NotSoSimpleEcommerce.Order.Domain.Models;
using NotSoSimpleEcommerce.Shared.Enums;
using NotSoSimpleEcommerce.Shared.InOut.Responses;

namespace NotSoSimpleEcommerce.Order.Domain.Mappings
{
    public static class ProductCommandToEntityMapping
    {
        public static OrderEntity MapToEntity(this CreateOrderCommand order)
        {
            return new OrderEntity
            (
                productId: order.ProductId,
                quantity: order.Quantity,
                boughtBy:order.BoughtBy,
                status: OrderStatus.Created
            );
        }
    }
}
