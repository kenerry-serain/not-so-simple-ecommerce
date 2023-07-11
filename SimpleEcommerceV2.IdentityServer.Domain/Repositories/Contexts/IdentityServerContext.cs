using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleEcommerceV2.IdentityServer.Domain.Models;

namespace SimpleEcommerceV2.IdentityServer.Domain.Repositories.Contexts
{
    public sealed class IdentityServerContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public IdentityServerContext(DbContextOptions<IdentityServerContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbSet<UserEntity> User { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var saveChangesResult =  base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.Clear();
            return saveChangesResult;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hashedPassword = SHA256.HashData(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Identity:Admin:User:Password")));
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Email = _configuration.GetValue<string>("Identity:Admin:User")!,
                    Password =Encoding.UTF8.GetString(hashedPassword)
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
