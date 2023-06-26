using SimpleEcommerce.Main.InOut.Responses;
using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Mappings
{
    internal static class EntityToResponseMapping
    {
        internal static StockResponse MapToResponse(this StockEntity stock)
        {
            return new StockResponse(stock.Id, stock.ProductId, stock.Quantity);
        }

        internal static ProductResponse MapToResponse(this ProductEntity product)
        {
            return new ProductResponse(product.Id, product.Name, product.Price);
        }

        internal static IEnumerable<ProductResponse> MapToResponse(this IEnumerable<ProductEntity> products)
        {
            return products.Select(product => new ProductResponse(product.Id, product.Name, product.Price));
        }
        
        internal static IEnumerable<StockResponse> MapToResponse(this IEnumerable<StockEntity> stocks)
        {
            return stocks.Select(stock => new StockResponse(stock.Id, stock.ProductId, stock.Quantity));
        }
    }
}
