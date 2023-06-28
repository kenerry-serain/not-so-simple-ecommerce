using Amazon.SimpleEmail;
using Amazon.SQS;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.MessageHandler.Implementations;
using SimpleEcommerceV2.MessageHandler.Models;
using SimpleEcommerceV2.Notificator.Domain.Abstractions;
using SimpleEcommerceV2.Notificator.Domain.Implementations;
using SimpleEcommerceV2.Notificator.Domain.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(applicationBuilder =>
{
    applicationBuilder.RegisterType<AwsSesEmailSender>().As<IEmailSender>();
    applicationBuilder.RegisterType<AmazonSimpleEmailServiceClient>().As<IAmazonSimpleEmailService>();
    applicationBuilder.RegisterType<AmazonSQSClient>().Named<IAmazonSQS>(nameof(IAmazonSQS));

    applicationBuilder.RegisterType<AwsSesEmailSenderProcessor>()
        .Named<IMessageProcessor>(nameof(AwsSesEmailSenderProcessor));
    
    applicationBuilder.RegisterType<AwsSqsQueueMonitor>()
        .As<IHostedService>()
        .WithParameter(
            new ResolvedParameter(
                (i, c) => i.ParameterType == typeof(AwsSqsQueueMonitorParams),
                (i, c) => c.Resolve<IOptionsSnapshot<AwsSqsQueueMonitorParams>>()
                    .Get("AwsSqsQueueMonitorParams01")
            )
        );

});

builder.Services.Configure<AwsSqsQueueMonitorParams>(
    "AwsSqsQueueMonitorParams01",
     builder.Configuration.GetSection("Notificator:AwsSqsQueueMonitorParams01")
);
builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.Map("/notificator", applicationBuilder =>
{
    applicationBuilder.UseSwagger();
    applicationBuilder.UseSwaggerUI();
    applicationBuilder.UseRouting();

    applicationBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health",
            new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
    });
});

app.Run();
