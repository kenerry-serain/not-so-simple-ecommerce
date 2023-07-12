using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Contexts;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Contracts;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Implementations;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Repositories.Implementations;

namespace NotSoSimpleEcommerce.Main.Modules
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
            builder.Register(_ =>
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
                    (i, _) => i.ParameterType == typeof(DbContext),
                    (_, c) => c.Resolve<ProductContext>())
            ).InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(UpdateEntityRepository<>))
                .As(typeof(IUpdateEntityRepository<>))
                .WithParameter(
                    new ResolvedParameter(
                        (i, _) => i.ParameterType == typeof(DbContext),
                        (_, c) => c.Resolve<ProductContext>())
                ).InstancePerLifetimeScope();

            builder
                    .RegisterGeneric(typeof(DeleteEntityRepository<>))
                    .As(typeof(IDeleteEntityRepository<>))
                    .WithParameter(
                        new ResolvedParameter(
                            (i, _) => i.ParameterType == typeof(DbContext),
                            (_, c) => c.Resolve<ProductContext>())
                    ).InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(ReadEntityRepository<>))
                .As(typeof(IReadEntityRepository<>))
                .WithParameter(
                    new ResolvedParameter(
                        (i, _) => i.ParameterType == typeof(DbContext),
                        (_, c) => c.Resolve<ProductContext>())
                ).InstancePerLifetimeScope();

            builder
                .RegisterType<StockReadRepository>()
                .As<IStockReadRepository>();
        }
    }
}
