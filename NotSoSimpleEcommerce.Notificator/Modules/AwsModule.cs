using Amazon.SimpleEmail;
using Amazon.SQS;
using Autofac;
using NotSoSimpleEcommerce.Notificator.Domain.Abstractions;
using NotSoSimpleEcommerce.Notificator.Domain.Implementations;

namespace NotSoSimpleEcommerce.Notificator.Modules;

public class AwsModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AmazonSQSClient>()
            .Named<IAmazonSQS>(nameof(IAmazonSQS));

        builder.RegisterType<AwsSesEmailSender>()
            .As<IEmailSender>();
        
        builder.RegisterType<AmazonSimpleEmailServiceClient>()
            .As<IAmazonSimpleEmailService>();
    }
}
