using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using SimpleEcommerceV2.Order.Domain.Commands;

namespace SimpleEcommerceV2.Order.Modules
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
