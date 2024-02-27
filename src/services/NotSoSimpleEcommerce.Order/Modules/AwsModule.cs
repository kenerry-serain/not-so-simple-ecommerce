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
            builder.Register(componentContext =>
            {
                var configuration = componentContext.Resolve<IConfiguration>();
                if (Convert.ToBoolean(configuration["LocalStack:IsEnabled"])){
                    var config = new AmazonSQSConfig
                    {
                        AuthenticationRegion = configuration["AWS_REGION"],
                        ServiceURL = configuration["LocalStack:ServiceURL"]
                    };

                    return new AmazonSQSClient(config);
                }

                var client = configuration
                    .GetAWSOptions()
                    .CreateServiceClient<IAmazonSQS>();

                return client;
            })
            .Named<IAmazonSQS>(nameof(IAmazonSQS))
            .SingleInstance();
                
            builder.RegisterType<AwsSqsMessageSender>()
                .As<IMessageSender>()
                .WithParameter(
                    new ResolvedParameter(
                        (i, _) => i.ParameterType == typeof(IAmazonSQS),
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
