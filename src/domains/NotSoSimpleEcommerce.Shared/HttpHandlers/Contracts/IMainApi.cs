using NotSoSimpleEcommerce.Shared.InOut.Responses;
using Refit;

namespace NotSoSimpleEcommerce.Shared.HttpHandlers.Contracts
{
    public interface IMainApi
    {
        [Put("/api/product/{id}/stock")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<object>> UpdateProductStockAsync(int id, [Body] object payload);

        [Get("/api/product/{id}/stock")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<StockResponse>> GetStockByProductIdAsync(int id);
    }
}
