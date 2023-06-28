using SimpleEcommerceV2.Main.Domain.InOut.Responses;
using SimpleEcommerceV2.Main.Domain.Models;

namespace SimpleEcommerceV2.Main.Domain.Mappings
{
    public static class EntityToResponseMapping
    {
        public static StockResponse MapToResponse(this StockEntity stock)
        {
            return new StockResponse(stock.Id, stock.ProductId, stock.Quantity);
        }

        public static ProductResponse MapToResponse(this ProductEntity product)
        {
            return new ProductResponse(product.Id, product.Name, product.Price);
        }

        public static IEnumerable<ProductResponse> MapToResponse(this IEnumerable<ProductEntity> products)
        {
            return products.Select(product => new ProductResponse(product.Id, product.Name, product.Price));
        }
        
        public static IEnumerable<StockResponse> MapToResponse(this IEnumerable<StockEntity> stocks)
        {
            return stocks.Select(stock => new StockResponse(stock.Id, stock.ProductId, stock.Quantity));
        }
    }
}
