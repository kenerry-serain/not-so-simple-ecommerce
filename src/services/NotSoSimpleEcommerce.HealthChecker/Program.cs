using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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
    Log.Information("Starting Health Checker Microservice");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services
        .AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Default")!, name: "Postgresql");

    builder.Services
        .AddHealthChecksUI()
        .AddInMemoryStorage();

    builder.Services.AddControllers();
    builder.Host.UseSerilog();

    var app = builder.Build();
    app.Map("/healthchecks", applicationBuilder =>
    {
        applicationBuilder.UseRouting();
        applicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecksUI(options =>
            {
                options.UIPath = "/ui";
            });
        });
        applicationBuilder.UseStaticFiles();
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
