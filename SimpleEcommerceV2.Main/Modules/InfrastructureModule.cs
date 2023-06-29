using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Main.Domain.Repositories.Contexts;
using SimpleEcommerceV2.Main.Domain.Repositories.Contracts;
using SimpleEcommerceV2.Main.Domain.Repositories.Implementations;
using SimpleEcommerceV2.Repositories.Contracts;
using SimpleEcommerceV2.Repositories.Implementations;

namespace SimpleEcommerceV2.Main.Modules
{
    public class InfrastructureModule: Module
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
                var options = new DbContextOptionsBuilder<ProductContext>();
                options.UseNpgsql(_configuration.GetConnectionString("Default"));
                var dbContext = new ProductContext(options.Options);
                dbContext.Database.Migrate();
                return dbContext;
            })
            .SingleInstance();

            builder
            .RegisterGeneric(typeof(CreateEntityRepository<>))
            .As(typeof(ICreateEntityRepository<>))
            .WithParameter(
                new ResolvedParameter(
                    (i, c) => i.ParameterType == typeof(DbContext),
                    (i, c) => c.Resolve<ProductContext>())
            ).InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(UpdateEntityRepository<>))
                .As(typeof(IUpdateEntityRepository<>))
                .WithParameter(
                    new ResolvedParameter(
                        (i, c) => i.ParameterType == typeof(DbContext),
                        (i, c) => c.Resolve<ProductContext>())
                ).InstancePerLifetimeScope();

            builder
                    .RegisterGeneric(typeof(DeleteEntityRepository<>))
                    .As(typeof(IDeleteEntityRepository<>))
                    .WithParameter(
                        new ResolvedParameter(
                            (i, c) => i.ParameterType == typeof(DbContext),
                            (i, c) => c.Resolve<ProductContext>())
                    ).InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(ReadEntityRepository<>))
                .As(typeof(IReadEntityRepository<>))
                .WithParameter(
                    new ResolvedParameter(
                        (i, c) => i.ParameterType == typeof(DbContext),
                        (i, c) => c.Resolve<ProductContext>())
                ).InstancePerLifetimeScope();

            builder
                .RegisterType<StockReadRepository>()
                .As<IStockReadRepository>();
        }
    }
}
