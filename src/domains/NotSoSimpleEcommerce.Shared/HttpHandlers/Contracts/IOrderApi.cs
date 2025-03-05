using NotSoSimpleEcommerce.Shared.InOut.Responses;
using Refit;

namespace NotSoSimpleEcommerce.Shared.HttpHandlers.Contracts;

public interface IOrderApi
{
    [Get("/api/request/{id}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<OrderResponse>> GetOrderByIdAsync(int id);
}
