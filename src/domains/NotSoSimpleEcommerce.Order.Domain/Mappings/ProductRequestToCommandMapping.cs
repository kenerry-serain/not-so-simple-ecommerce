using NotSoSimpleEcommerce.Order.Domain.Commands;
using NotSoSimpleEcommerce.Shared.InOut.Requests;

namespace NotSoSimpleEcommerce.Order.Domain.Mappings
{
    public static class ProductRequestToCommandMapping
    {
        public static CreateOrderCommand MapToRegisterOrderCommand(this OrderRequest order)
        {
            return new CreateOrderCommand
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
        }
        
        public static UpdateOrderCommand MapToUpdateCommand(this OrderRequest product, int id)
        {
            return new UpdateOrderCommand
            {
                Id = id,
                ProductId = product.ProductId,
                Quantity = product.Quantity
            };
        }
    }
}
