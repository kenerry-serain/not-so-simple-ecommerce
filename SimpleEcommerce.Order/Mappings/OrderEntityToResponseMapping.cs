using SimpleEcommerce.Order.InOut.Responses;
using SimpleEcommerce.Order.Models;

namespace SimpleEcommerce.Order.Mappings
{
    internal static class OrderEntityToResponseMapping
    {
        internal static OrderResponse MapToResponse(this OrderEntity order)
        {
            return new OrderResponse(order.Id, order.ProductId, order.Quantity);
        }
    }
}
