using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.InOut.Requests;

namespace SimpleEcommerce.Main.Mappings
{
    internal static class RequestToCommandMapping
    {
        internal static RegisterProductStockCommand MapToRegisterProductStockCommand(this StockRequest stock)
        {
            return new RegisterProductStockCommand
            {
                ProductId = stock.ProductId,
                Quantity = stock.Quantity
            };
        }

        internal static UpdateProductStockCommand MapToUpdateProductStockCommand(this StockRequest stock)
        {
            return new UpdateProductStockCommand
            {
                ProductId = stock.ProductId,
                Quantity = stock.Quantity
            };
        }

        internal static RegisterProductCommand MapToRegisterCommand(this ProductRequest product)
        {
            return new RegisterProductCommand
            {
                Name = product.Name,
                Price = product.Price
            };
        }

        internal static UpdateProductCommand MapToUpdateCommand(this ProductRequest product, int id)
        {
            return new UpdateProductCommand
            {
                Id = id,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
