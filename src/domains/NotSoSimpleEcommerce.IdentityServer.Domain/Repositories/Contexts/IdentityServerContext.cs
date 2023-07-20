using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotSoSimpleEcommerce.IdentityServer.Domain.Models;
using NotSoSimpleEcommerce.IdentityServer.Domain.Repositories.Configurations;

namespace NotSoSimpleEcommerce.IdentityServer.Domain.Repositories.Contexts
{
    public sealed class IdentityServerContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public IdentityServerContext(DbContextOptions<IdentityServerContext> options, IConfiguration configuration) :
            base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbSet<UserEntity> User { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var password =_configuration.GetValue<string>("Identity:Admin:User:Password")!;
            var hashedPassword = SHA256.HashData(Encoding.UTF8.GetBytes(password));
     
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
               
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                (
                    id:1,
                    email: _configuration.GetValue<string>("Identity:Admin:User")!,
                    password:Encoding.UTF8.GetString(hashedPassword)
                )
            );
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
