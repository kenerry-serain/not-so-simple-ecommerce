using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Options;
using NotSoSimpleEcommerce.InvoiceGenerator.Tasks;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Implementations;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.InvoiceGenerator.Modules;

public class DomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<InvoiceProcessor>()
            .Named<IMessageProcessor>(nameof(InvoiceProcessor));
        
        builder.RegisterType<AwsSqsQueueMonitor>()
            .As<IHostedService>()
            .WithParameter(
                new ResolvedParameter(
                    (i, _) => i.ParameterType == typeof(AwsSqsQueueMonitorParams),
                    (_, c) => c.Resolve<IOptionsSnapshot<AwsSqsQueueMonitorParams>>()
                        .Get("AwsSqsQueueMonitorParams01")
                )
            );
    }
}
