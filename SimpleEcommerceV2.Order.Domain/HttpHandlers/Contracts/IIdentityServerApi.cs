using Refit;
using SimpleEcommerceV2.Order.Domain.InOut.Requests;
using SimpleEcommerceV2.Order.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Order.Domain.HttpHandlers.Contracts
{
    public interface IIdentityServerApi
    {
        [Post("/identity/api/auth")]
        Task<string> AuthAsync(AuthRequest userRequest);
    }
}
