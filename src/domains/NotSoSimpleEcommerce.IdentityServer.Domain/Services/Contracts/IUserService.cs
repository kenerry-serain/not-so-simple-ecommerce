using NotSoSimpleEcommerce.Shared.InOut.Requests;

namespace NotSoSimpleEcommerce.IdentityServer.Domain.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(AuthRequest userRequest);
    }
}
