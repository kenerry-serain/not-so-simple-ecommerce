using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.IdentityServer.Domain.Models;
using NotSoSimpleEcommerce.IdentityServer.Domain.Services.Contracts;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.InOut.Requests;

namespace NotSoSimpleEcommerce.IdentityServer.Domain.Services.Implementations
{
    public sealed class UserService : IUserService
    {
        private readonly IReadEntityRepository<UserEntity> _readRepository;
        public UserService(IReadEntityRepository<UserEntity> readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public async Task<bool> CheckPasswordAsync(AuthRequest userRequest)
        {
            var user = await _readRepository.GetAll()
                .FirstOrDefaultAsync(user => user.Email == userRequest.Email);

            if (user is null)
                throw new KeyNotFoundException("User not found.");

            var hashedPassword = Encoding.UTF8.GetString(SHA256.HashData(Encoding.UTF8.GetBytes(userRequest.Password)));
            return user.Password == hashedPassword;
        }
    }
}
