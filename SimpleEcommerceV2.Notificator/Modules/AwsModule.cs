using Amazon.SimpleEmail;
using Autofac;
using SimpleEcommerceV2.Notificator.Domain.Abstractions;
using SimpleEcommerceV2.Notificator.Domain.Implementations;

namespace SimpleEcommerceV2.Notificator.Modules;

public class AwsModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        
        builder.RegisterType<AwsSesEmailSender>().As<IEmailSender>();
        builder.RegisterType<AmazonSimpleEmailServiceClient>().As<IAmazonSimpleEmailService>();
    }
}
