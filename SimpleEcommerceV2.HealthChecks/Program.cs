using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Default"), name: "Postgresql");

builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();

builder.Services.AddControllers();

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
