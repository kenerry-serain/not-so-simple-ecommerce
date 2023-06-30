using SimpleEcommerceV2.IdentityServer.Domain.InOut;

namespace SimpleEcommerceV2.IdentityServer.Domain.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(AuthRequest userRequest);
    }
}
