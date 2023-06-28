using SimpleEcommerceV2.Order.Domain.InOut.Responses;
using SimpleEcommerceV2.Order.Domain.Models;

namespace SimpleEcommerceV2.Order.Domain.Mappings
{
    public static class OrderEntityToResponseMapping
    {
        public static OrderResponse MapToResponse(this OrderEntity order)
        {
            return new OrderResponse(order.Id, order.ProductId, order.Quantity);
        }
    }
}
