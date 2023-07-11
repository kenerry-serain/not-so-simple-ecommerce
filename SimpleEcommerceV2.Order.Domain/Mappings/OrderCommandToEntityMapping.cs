using SimpleEcommerceV2.Order.Domain.Commands;
using SimpleEcommerceV2.Order.Domain.Models;

namespace SimpleEcommerceV2.Order.Domain.Mappings
{
    public static class ProductCommandToEntityMapping
    {
        public static OrderEntity MapToEntity(this CreateOrderCommand order)
        {
            return new OrderEntity
            {
                Quantity = order.Quantity,
                ProductId = order.ProductId
            };
        }
    }
}
