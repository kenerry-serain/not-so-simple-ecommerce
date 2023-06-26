using SimpleEcommerce.Order.Commands;
using SimpleEcommerce.Order.InOut.Requests;

namespace SimpleEcommerce.Order.Mappings
{
    internal static class ProductRequestToCommandMapping
    {
        internal static RegisterOrderCommand MapToRegisterOrderCommand(this OrderRequest order)
        {
            return new RegisterOrderCommand
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
        }

    }
}
