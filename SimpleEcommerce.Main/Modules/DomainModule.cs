using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using SimpleEcommerceV2.Main.Domain.Commands;

namespace SimpleEcommerceV2.Main.Modules
{
    public class DomainModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = MediatRConfigurationBuilder
                  .Create(typeof(RegisterProductCommand).Assembly)
                  .WithAllOpenGenericHandlerTypesRegistered()
                  .Build();

            builder.RegisterMediatR(configuration);
        }
    }
}
