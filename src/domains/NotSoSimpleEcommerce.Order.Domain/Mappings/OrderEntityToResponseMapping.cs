using NotSoSimpleEcommerce.Shared.InOut.Responses;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Mappings
{
    public static class OrderEntityToResponseMapping
    {
        public static OrderResponse MapToResponse(this OrderEntity order)
        {
            return new OrderResponse
            (
                order.Id, 
                order.Product.MapToResponse(), 
                order.Quantity, 
                order.BoughtBy, 
                order.StatusId,
                Enum.GetName(order.StatusId)
            );
        }
        
        public static ProductResponse MapToResponse(this ProductEntity product)
        {
            if (product is null)
                return null;
            return new ProductResponse(product.Id, product.Name, product.Price);
        }
        
        public static IEnumerable<OrderResponse> MapToResponse(this IEnumerable<OrderEntity> orders)
        {
            return orders.Select(order => order.MapToResponse());
        }
    }
}
