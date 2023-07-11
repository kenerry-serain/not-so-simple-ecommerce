using SimpleEcommerceV2.Order.Domain.Commands;
using SimpleEcommerceV2.Shared.InOut.Requests;

namespace SimpleEcommerceV2.Order.Domain.Mappings
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

    }
}
