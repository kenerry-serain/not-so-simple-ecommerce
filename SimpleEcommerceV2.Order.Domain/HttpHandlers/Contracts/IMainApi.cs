using Refit;
using SimpleEcommerceV2.Order.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Order.Domain.HttpHandlers.Contracts
{
    public interface IMainApi
    {
        [Put("/main/api/product/{id}/stock")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<object>> UpdateAsync(int id, [Body] object payload);

        [Get("/main/api/product/{id}/stock")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<StockResponse>> GetStockByProductIdAsync(int id);
    }
}
