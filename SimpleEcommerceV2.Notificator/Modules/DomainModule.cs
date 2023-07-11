using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Options;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.MessageHandler.Implementations;
using SimpleEcommerceV2.MessageHandler.Models;
using SimpleEcommerceV2.Notificator.Domain.Tasks;

namespace SimpleEcommerceV2.Notificator.Modules;

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
