using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using NotSoSimpleEcommerce.Main.Domain.Commands;

namespace NotSoSimpleEcommerce.Main.Modules
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
