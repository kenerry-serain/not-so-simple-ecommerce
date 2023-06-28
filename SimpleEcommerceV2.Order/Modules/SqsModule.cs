using Autofac;
using Amazon.SQS;
using Autofac.Core;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.MessageHandler.Implementations;
using SimpleEcommerceV2.MessageHandler.Models;
using Microsoft.Extensions.Options;

namespace SimpleEcommerceV2.Order.Modules
{
    public class SqsModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AmazonSQSClient>()
                .Named<IAmazonSQS>(nameof(IAmazonSQS));

            builder.RegisterType<AwsSqsMessageSender>()
                .As<IMessageSender>()
                .WithParameter(
                    new ResolvedParameter(
                        (i, c) => i.ParameterType == typeof(IAmazonSQS),
                        (i, c) => c.ResolveNamed<IAmazonSQS>(nameof(IAmazonSQS))))
                .WithParameter(
                    new ResolvedParameter(
                        (i, c) => i.ParameterType == typeof(AwsSqsMessageParams),
                        (i, c) => c.Resolve<IOptionsSnapshot<AwsSqsMessageParams>>()
                            .Get("AwsSqsMessageSenderParams01"))
                );
        }
    }
}
