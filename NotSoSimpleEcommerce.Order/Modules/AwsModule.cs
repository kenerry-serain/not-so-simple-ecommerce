using Amazon.SQS;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Options;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Implementations;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.Order.Modules
{
    public class AwsModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AmazonSQSClient>()
                .As<IAmazonSQS>();
                
            builder.RegisterType<AwsSqsMessageSender>()
                .As<IMessageSender>()
                .WithParameter(
                    new ResolvedParameter(
                        (i, _) => i.ParameterType == typeof(IMessageSender),
                        (_, c) => c.ResolveNamed<IAmazonSQS>(nameof(IAmazonSQS)))
                )
                .WithParameter(
                    new ResolvedParameter(
                        (i, _) => i.ParameterType == typeof(AwsSqsMessageSenderParams),
                        (_, c) => c.Resolve<IOptionsSnapshot<AwsSqsMessageSenderParams>>()
                            .Get("AwsSqsMessageSenderParams01"))
                );
        }
    }
}
