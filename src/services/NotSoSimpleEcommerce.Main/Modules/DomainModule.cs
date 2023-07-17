using Autofac;
using Autofac.Core;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Options;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Main.Domain.Tasks;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Implementations;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.Main.Modules
{
    public class DomainModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductStockProcessor>()
                .Named<IMessageProcessor>(nameof(ProductStockProcessor));
        
            builder.RegisterType<AwsSqsQueueMonitor>()
                .As<IHostedService>()
                .WithParameter(
                    new ResolvedParameter(
                        (i, _) => i.ParameterType == typeof(AwsSqsQueueMonitorParams),
                        (_, c) => c.Resolve<IOptionsSnapshot<AwsSqsQueueMonitorParams>>()
                            .Get("AwsSqsQueueMonitorParams01")
                    )
                );
            
            var configuration = MediatRConfigurationBuilder
                  .Create(typeof(RegisterProductCommand).Assembly)
                  .WithAllOpenGenericHandlerTypesRegistered()
                  .Build();

            builder.RegisterMediatR(configuration);
        }
    }
}
