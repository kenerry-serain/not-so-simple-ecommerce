using Refit;
using SimpleEcommerceV2.Shared.InOut.Responses;

namespace SimpleEcommerceV2.Shared.HttpHandlers.Contracts
{
    public interface IMainApi
    {
        [Put("/main/api/product/{id}/stock")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<object>> UpdateProductStockAsync(int id, [Body] object payload);

        [Get("/main/api/product/{id}/stock")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<StockResponse>> GetStockByProductIdAsync(int id);
    }
}
