using Amazon.SQS;
using Autofac;

namespace NotSoSimpleEcommerce.InvoiceGenerator.Modules;

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
    }
}
