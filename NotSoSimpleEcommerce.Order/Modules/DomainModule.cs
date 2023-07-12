using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using NotSoSimpleEcommerce.Order.Domain.Commands;

namespace NotSoSimpleEcommerce.Order.Modules
{
    public class DomainModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = MediatRConfigurationBuilder
                       .Create(typeof(CreateOrderCommand).Assembly)
                       .WithAllOpenGenericHandlerTypesRegistered()
                       .Build();

            builder.RegisterMediatR(configuration);
        }
    }
}
