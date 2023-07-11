using Refit;
using SimpleEcommerceV2.Shared.InOut.Requests;

namespace SimpleEcommerceV2.Shared.HttpHandlers.Contracts
{
    public interface IIdentityServerApi
    {
        [Post("/identity/api/auth")]
        Task<string> AuthAsync(AuthRequest userRequest);
    }
}
