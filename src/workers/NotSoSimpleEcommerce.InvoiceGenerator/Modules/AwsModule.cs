using Amazon.SQS;
using Autofac;

namespace NotSoSimpleEcommerce.InvoiceGenerator.Modules;

public class AwsModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AmazonSQSClient>()
            .Named<IAmazonSQS>(nameof(IAmazonSQS));
    }
}
