using NotSoSimpleEcommerce.Shared.InOut.Responses;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Mappings
{
    public static class OrderEntityToResponseMapping
    {
        public static OrderResponse MapToResponse(this OrderEntity order)
        {
            return new OrderResponse(order.Id, order.ProductId, order.Quantity, order.BoughtBy, order.StatusId);
        }
        
        public static IEnumerable<OrderResponse> MapToResponse(this IEnumerable<OrderEntity> orders)
        {
            return orders.Select(order => order.MapToResponse());
        }
    }
}
