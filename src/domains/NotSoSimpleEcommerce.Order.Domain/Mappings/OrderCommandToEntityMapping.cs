using NotSoSimpleEcommerce.Order.Domain.Commands;
using NotSoSimpleEcommerce.Shared.Enums;
using NotSoSimpleEcommerce.Shared.Models;

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
                statusId: OrderStatus.Pendente
            );
        }
        
        public static OrderEntity MapToEntity(this UpdateOrderCommand order)
        {
            return new OrderEntity
            (
                productId: order.ProductId,
                quantity: order.Quantity,
                boughtBy:order.BoughtBy,
                statusId: OrderStatus.Pendente
            );
        }
    }
}
