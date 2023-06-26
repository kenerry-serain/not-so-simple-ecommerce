using Refit;

namespace SimpleEcommerce.Order.HttpHandlers.Contracts
{
    public interface ISimpleHttpClient
    {
        [Put("/main/api/product/{id}/stock")]
        Task<ApiResponse<object>> UpdateAsync(int id, [Body] object payload);
    }
}
