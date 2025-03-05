using NotSoSimpleEcommerce.Shared.InOut.Requests;
using Refit;

namespace NotSoSimpleEcommerce.Shared.HttpHandlers.Contracts
{
    public interface IIdentityServerApi
    {
        [Post("/api/auth")]
        Task<string> AuthAsync(AuthRequest userRequest);
    }
}
