using SimpleEcommerce.Order.Commands;
using SimpleEcommerce.Order.Models;

namespace SimpleEcommerce.Order.Mappings
{
    internal static class ProductCommandToEntityMapping
    {
        internal static OrderEntity MapToEntity(this RegisterOrderCommand order)
        {
            return new OrderEntity
            {
                Quantity = order.Quantity,
                ProductId = order.ProductId
            };
        }
    }
}
