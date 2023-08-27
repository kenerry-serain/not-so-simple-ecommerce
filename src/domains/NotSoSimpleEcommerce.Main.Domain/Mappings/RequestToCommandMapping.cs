using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Main.Domain.InOut.Requests;

namespace NotSoSimpleEcommerce.Main.Domain.Mappings
{
    public static class RequestToCommandMapping
    {
        public static RegisterProductStockCommand MapToRegisterProductStockCommand(this StockRequest stock, int productId)
        {
            return new RegisterProductStockCommand
            {
                ProductId = productId,
                Quantity = stock.Quantity
            };
        }

        public static UpdateProductStockCommand MapToUpdateProductStockCommand(this StockRequest stock, int productId)
        {
            return new UpdateProductStockCommand
            {
                ProductId = productId,
                Quantity = stock.Quantity
            };
        }

        public static RegisterProductCommand MapToRegisterCommand(this ProductRequest product)
        {
            return new RegisterProductCommand
            {
                Name = product.Name,
                Price = product.Price
            };
        }

        public static UpdateProductCommand MapToUpdateCommand(this ProductRequest product, int id)
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
