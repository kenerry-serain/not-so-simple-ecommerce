using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.Mappings
{
    public static class CommandToEntityMapping
    {
        public static StockEntity MapToEntity(this RegisterProductStockCommand stock)
        {
            return new StockEntity
            {
                Quantity = stock.Quantity,
                ProductId = stock.ProductId
            };
        }

        public static ProductEntity MapToEntity(this RegisterProductCommand product)
        {
            return new ProductEntity
            {
                Name = product.Name,
                Price = product.Price
            };
        }

        public static ProductEntity MapToEntity(this UpdateProductCommand product)
        {
            return new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public static StockEntity MapToEntity(this UpdateProductStockCommand product)
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
