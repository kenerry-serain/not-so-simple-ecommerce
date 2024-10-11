using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotSoSimpleEcommerce.Order.Middlewares;
using NotSoSimpleEcommerce.Order.Modules;
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
    Log.Information("Starting Order Microservice");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(applicationBuilder =>
    {
        applicationBuilder.RegisterModule<DomainModule>();
        applicationBuilder.RegisterModule<AwsModule>();
        applicationBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));

        applicationBuilder
            .RegisterType<GlobalErrorHandlerMiddleware>()
            .SingleInstance();
    });

    builder.Services.Configure<AwsSqsMessageSenderParams>(
        "AwsSqsMessageSenderParams01",
        builder.Configuration.GetSection("Order:AwsSqsMessageSenderParams01")
    );

    builder.Services.Configure<AwsSqsMessageSenderParams>(
        "AwsSqsMessageSenderParams02",
        builder.Configuration.GetSection("Order:AwsSqsMessageSenderParams02")
    );

    builder.Services.AddMemoryCache();
    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Provide a token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        var passwordBytes = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Identity:Key")!);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetValue<string>("Identity:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Identity:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(passwordBytes),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
    builder.Services.AddAuthorization();
    builder.Services.AddHttpContextAccessor();

    builder.Host.UseSerilog();

    var app = builder.Build();

    app.Map("/order", applicationBuilder =>
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
                    Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
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
