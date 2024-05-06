using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NotSoSimpleEcommerce.InvoiceGenerator.Modules;
using NotSoSimpleEcommerce.SqsHandler.Models;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(LogEventLevel.Information)
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

try
{
    Log.Information("Starting Invoice Generator Microservice");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(applicationBuilder =>
    {
        applicationBuilder.RegisterModule<AwsModule>();
        applicationBuilder.RegisterModule<DomainModule>();
    });

    builder.Services.Configure<AwsSqsQueueMonitorParams>(
        "AwsSqsQueueMonitorParams01",
        builder.Configuration.GetSection("InvoiceGenerator:AwsSqsQueueMonitorParams01")
    );

    builder.Services.Configure<AwsSqsMessageSenderParams>(
        "AwsSqsMessageSenderParams01",
        builder.Configuration.GetSection("InvoiceGenerator:AwsSqsMessageSenderParams01")
    );


    builder.Services.AddMemoryCache();
    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();

    app.Map("/invoice", applicationBuilder =>
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
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
