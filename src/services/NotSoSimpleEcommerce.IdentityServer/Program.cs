using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NotSoSimpleEcommerce.IdentityServer.Middlewares;
using NotSoSimpleEcommerce.IdentityServer.Modules;
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
    Log.Information("Starting Identity Server Microservice");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(applicationBuilder =>
    {
        applicationBuilder.RegisterModule<DomainModule>();
        applicationBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));

        applicationBuilder
            .RegisterType<GlobalErrorHandlerMiddleware>()
            .SingleInstance();
    });
    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.Map("/identity", applicationBuilder =>
    {
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI();
        applicationBuilder.UseRouting();
        applicationBuilder.UseMiddleware<GlobalErrorHandlerMiddleware>();
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
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
catch (HostAbortedException exception)
{
    Log.Warning(exception, "Executing migrations? All good.");
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
