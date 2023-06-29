using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.IdentityServer.Domain.Repositories.Contexts;
using SimpleEcommerceV2.Repositories.Contracts;
using SimpleEcommerceV2.Repositories.Implementations;

namespace SimpleEcommerceV2.IdentityServer.Modules
{
    public class InfrastructureModule : Module
    {
        private readonly IConfiguration _configuration;

        public InfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(configuration =>
            {
                var options = new DbContextOptionsBuilder<IdentityServerContext>();
                options.UseNpgsql(_configuration.GetConnectionString("Default"));
                var dbContext = new IdentityServerContext(options.Options, _configuration);
                dbContext.Database.Migrate();
                return dbContext;
            })
            .SingleInstance();

            builder
                .RegisterGeneric(typeof(ReadEntityRepository<>))
                .As(typeof(IReadEntityRepository<>))
                .WithParameter(
                    new ResolvedParameter(
                        (i, c) => i.ParameterType == typeof(DbContext),
                        (i, c) => c.Resolve<IdentityServerContext>())
                )
                .InstancePerLifetimeScope();
        }
    }
}
