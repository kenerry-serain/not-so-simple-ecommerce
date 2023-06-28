using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using Refit;
using SimpleEcommerceV2.Order.Domain.HttpHandlers.Contracts;
using SimpleEcommerceV2.Order.Domain.Repositories.Contexts;
using SimpleEcommerceV2.Repositories.Contracts;
using SimpleEcommerceV2.Repositories.Implementations;

namespace SimpleEcommerceV2.Order.Modules
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
                var options = new DbContextOptionsBuilder<OrderContext>();
                options.UseNpgsql(_configuration.GetConnectionString("Default"));
                var dbContext = new OrderContext(options.Options);

                dbContext.Database.Migrate();
                return dbContext;
            })
            .InstancePerLifetimeScope();

            builder
              .RegisterGeneric(typeof(CreateEntityRepository<>))
              .As(typeof(ICreateEntityRepository<>))
              .WithParameter(
                  new ResolvedParameter(
                      (i, c) => i.ParameterType == typeof(DbContext),
                      (i, c) => c.Resolve<OrderContext>())
              ).InstancePerLifetimeScope();

            builder.Register(_ => RestService.For<ISimpleHttpClient>(
                _configuration.GetValue<string>("BaseAddress:Product"))
            );
        }
    }
}
