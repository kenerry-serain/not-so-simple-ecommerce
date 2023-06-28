using Refit;

namespace SimpleEcommerceV2.Order.Domain.HttpHandlers.Contracts
{
    public interface ISimpleHttpClient
    {
        [Put("/main/api/product/{id}/stock")]
        Task<ApiResponse<object>> UpdateAsync(int id, [Body] object payload);
    }
}
