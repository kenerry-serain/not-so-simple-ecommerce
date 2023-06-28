using SimpleEcommerceV2.Order.Domain.Commands;
using SimpleEcommerceV2.Order.Domain.InOut.Requests;

namespace SimpleEcommerceV2.Order.Domain.Mappings
{
    public static class ProductRequestToCommandMapping
    {
        public static RegisterOrderCommand MapToRegisterOrderCommand(this OrderRequest order)
        {
            return new RegisterOrderCommand
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
        }

    }
}
