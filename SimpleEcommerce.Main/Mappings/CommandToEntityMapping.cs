using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Mappings
{
    internal static class CommandToEntityMapping
    {
        internal static StockEntity MapToEntity(this RegisterProductStockCommand stock)
        {
            return new StockEntity
            {
                Quantity = stock.Quantity,
                ProductId = stock.ProductId
            };
        }

        internal static ProductEntity MapToEntity(this RegisterProductCommand product)
        {
            return new ProductEntity
            {
                Name = product.Name,
                Price = product.Price
            };
        }

        internal static ProductEntity MapToEntity(this UpdateProductCommand product)
        {
            return new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        internal static StockEntity MapToEntity(this UpdateProductStockCommand product)
        {
            return new StockEntity
            {
                Id = product.Id,
                ProductId = product.ProductId,
                Quantity = product.Quantity
            };
        }
    }
}
