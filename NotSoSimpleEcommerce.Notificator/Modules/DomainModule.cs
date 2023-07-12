using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Options;
using NotSoSimpleEcommerce.MessageHandler.Abstractions;
using NotSoSimpleEcommerce.MessageHandler.Implementations;
using NotSoSimpleEcommerce.MessageHandler.Models;
using NotSoSimpleEcommerce.Notificator.Domain.Tasks;

namespace NotSoSimpleEcommerce.Notificator.Modules;

public class DomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AwsSesEmailSenderProcessor>()
            .Named<IMessageProcessor>(nameof(AwsSesEmailSenderProcessor));

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
